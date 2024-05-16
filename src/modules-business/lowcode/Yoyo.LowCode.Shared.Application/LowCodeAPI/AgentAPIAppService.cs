// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Masuit.Tools;
using Microsoft.EntityFrameworkCore;
using Yoyo.LowCode.LowCodeAPI.DomainService;
using Yoyo.LowCode.LowCodeAPI.Dtos.AgentAPIDto;
using Yoyo.LowCode.LowCodeAPI.Dtos.APIRequestRecordDto;
using Yoyo.LowCode.LowCodeAPI.Dtos.AppAuthDto;
using Yoyo.LowCode.LowCodeAPI.Dtos.FlowControlDto;

namespace Yoyo.LowCode.LowCodeAPI
{
    public class AgentAPIAppService : LowCodeSharedAppServiceBase, IAgentAPIAppService
    {
        #region Private Fields

        private readonly IUnitOfWorkManager _unitOfWorkManager;

        private readonly IApplyBodyDataManager _bodyDataManager;
        private readonly IApplyQHDataManager _qhDataManager;
        private readonly IParameterMappingManager _parameterMapping;
        private readonly IAPIRequestRecordManager _apiLog;

        private readonly IAgentAPIManager _agentAPIManager;
        private readonly IAppAuthManager _authManager;
        private readonly IAppAuthMappingManager _appAuthMapping;
        private readonly IFlowControlMappingManager _flowControlMapping;

        #endregion Private Fields

        #region Public Constructors

        public AgentAPIAppService(IAgentAPIManager agentAPI,
            IAppAuthManager appAuth,
            IAppAuthMappingManager appAuthMapping,
            IUnitOfWorkManager unitOfWorkManager,
            IApplyBodyDataManager applyBodyDataManager,
            IApplyQHDataManager applyQHDataManager,
            IParameterMappingManager parameterMapping,
            IAPIRequestRecordManager apiLog,
            IFlowControlMappingManager flowControlMapping)
        {
            _agentAPIManager = agentAPI;
            _authManager = appAuth;
            _appAuthMapping = appAuthMapping;
            _unitOfWorkManager = unitOfWorkManager;
            _bodyDataManager = applyBodyDataManager;
            _qhDataManager = applyQHDataManager;
            _parameterMapping = parameterMapping;
            _apiLog = apiLog;
            _flowControlMapping = flowControlMapping;
        }

        #endregion Public Constructors

        /// <summary>
        /// 批量删除接口中心API接口数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task BatchDelete(List<Guid> input)
        {
            await _agentAPIManager.BatchDelete(input);
        }

        /// <summary>
        /// 新增修改接口中心API接口数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="Abp.UI.UserFriendlyException"></exception>
        public async Task CreateOrUpdate(CreateOrUpdateAgentAPI input)
        {
            var applyTypeList = await _agentAPIManager.QueryAsNoTracking.Where(t => t.Id != input.AgentAPIEditDto.Id).ToListAsync();

            if (applyTypeList.Any(t => t.Name == input.AgentAPIEditDto.Name))
            {
                throw new Abp.UI.UserFriendlyException(L($"当前API名称{input.AgentAPIEditDto.Name}已被使用，请重新输入名称"));
            }

            if (applyTypeList.Any(t => t.Path == input.AgentAPIEditDto.Path))
            {
                throw new Abp.UI.UserFriendlyException(L($"当前API路径{input.AgentAPIEditDto.Name}已被使用，请重新输入路径"));
            }

            var entity = ObjectMapper.Map<AgentAPI>(input.AgentAPIEditDto);

            ///新增还是修改
            if (input.AgentAPIEditDto.Id.HasValue)
            {
                await _agentAPIManager.UpdateAsync(entity);
            }
            else
            {
                await _agentAPIManager.CreateAsync(entity);
            }
        }

        /// <summary>
        /// 删除 接口中心API接口数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task Delete(EntityDto<Guid> input)
        {
            await _agentAPIManager.Delete(input.Id);
        }

        /// <summary>
        /// 获取接口中心API接口数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<AgentAPIListDto>> GetAgentAPIList(GetAgentAPIPagesInput input)
        {
            var query = _agentAPIManager.Query
                .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a =>
                  a.Name.Contains(input.FilterText) ||
                  a.Path.Contains(input.FilterText)
              );
            var count = await query.CountAsync();

            var DataList = await query
                .OrderBy(input.Sorting).AsNoTracking()
                .PageBy(input)
                .ToListAsync();

            var DataListDtos = ObjectMapper.Map<List<AgentAPIListDto>>(DataList);

            return new PagedResultDto<AgentAPIListDto>(count, DataListDtos);
        }

        /// <summary>
        /// 根据ID 获取接口中心API接口数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<AgentAPIListDto> GetById(EntityDto<Guid> input)
        {
            var entity = await _agentAPIManager
            .QueryAsNoTracking
            .FirstOrDefaultAsync(x => x.Id == input.Id);

            var dto = ObjectMapper.Map<AgentAPIListDto>(entity);
            return dto;
        }

        /// <summary>
        /// 根据 请求路径 查询API信息
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task<AgentAPIListDto> GetAgentAPIByPath(string path)
        {
            var entity = await _agentAPIManager
            .QueryAsNoTracking
            .FirstOrDefaultAsync(x => x.Path == path);

            var dto = ObjectMapper.Map<AgentAPIListDto>(entity);
            return dto;
        }

        /// <summary>
        /// 返回所有接口信息(不分页)
        /// </summary>
        /// <returns></returns>
        public async Task<List<AgentAPIListAllDto>> GetAgentAPIListAll(GetAgentAPIPagesInput input)
        {
            var DataList = await _agentAPIManager.Query
                .WhereIf(!input.AgentName.IsNullOrWhiteSpace(), x => x.Name.Contains(input.AgentName))
                .ToListAsync();

            var DataListDtos = ObjectMapper.Map<List<AgentAPIListAllDto>>(DataList);

            if (DataList?.Count > 0)
            {
                if (input.CheckType == 0)
                {
                    //mapping关系 是否存在 （需要改写成连接查询）
                    var mappingList = await _appAuthMapping.Query
                      .Where(t => t.AppAuthID == input.AppAuthID)
                   .ToListAsync();
                    //转一下
                    var mappingsDtos = ObjectMapper.Map<List<AppAuthMappingListDto>>(mappingList);

                    foreach (var item in DataListDtos)
                    {
                        var mapping = mappingsDtos.Where(t => t.AgentID == item.id).FirstOrDefault();
                        if (mapping != null)
                        {
                            item.IsCheck = true;
                            item.AppAuthMapping = mapping;
                        }
                    }
                }
                else
                {
                    //mapping关系 是否存在 （需要改写成连接查询）
                    var mappingList = await _flowControlMapping.Query
                      .Where(t => t.FlowID == input.FlowID)
                   .ToListAsync();
                    //转一下
                    var mappingsDtos = ObjectMapper.Map<List<FlowControlMappingListDto>>(mappingList);

                    foreach (var item in DataListDtos)
                    {
                        var mapping = mappingsDtos.Where(t => t.AgentID == item.id).FirstOrDefault();
                        if (mapping != null)
                        {
                            item.IsCheck = true;
                            item.FlowMapping = mapping;
                        }
                    }
                }
            }
            return DataListDtos;
        }

        /// <summary>
        /// 添加或者修改 接口中心API接口数据(接口中心API 请求query header body 后端映射关系)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="Abp.UI.UserFriendlyException"></exception>
        public async Task CreateOrUpdateAgentAPI(CreateOrUpdateAgentAPI input)
        {
            bool isCreate = false;
            Guid agentID = Guid.NewGuid();
            var agentApiList = await _agentAPIManager.QueryAsNoTracking.Where(t => t.Id != input.AgentAPIEditDto.Id).ToListAsync();
            if (agentApiList.Any(t => t.Name == input.AgentAPIEditDto.Name))
            {
                throw new Abp.UI.UserFriendlyException(L($"当前API名称{input.AgentAPIEditDto.Name}已被使用，请重新输入名称"));
            }

            if (agentApiList.Any(t => t.Path == input.AgentAPIEditDto.Path))
            {
                throw new Abp.UI.UserFriendlyException(L($"当前API路径{input.AgentAPIEditDto.Name}已被使用，请重新输入路径"));
            }

            var agentApiEntity = ObjectMapper.Map<AgentAPI>(input.AgentAPIEditDto);

            ///新增还是修改
            if (input.AgentAPIEditDto.Id.HasValue)
            {
                agentID = (Guid)input.AgentAPIEditDto.Id;
                isCreate = false;
            }
            else
            {
                agentID = Guid.NewGuid();
                isCreate = true;
            }

            ///处理query
            var query_ids = _qhDataManager.QueryAsNoTracking
                .Where(t => t.APIID == agentID && t.DataType.ToLower() == "query".ToLower())
                .Select(t => t.Id).ToList();
            var query_entity = ObjectMapper.Map<List<ApplyQHData>>(input.QueryData.Where(t => t.DataType.ToLower() == "query".ToLower()));

            ///处理header
            var header_ids = _qhDataManager.QueryAsNoTracking
              .Where(t => t.APIID == agentID && t.DataType.ToLower() == "Header".ToLower())
              .Select(t => t.Id).ToList();
            var header_entity = ObjectMapper.Map<List<ApplyQHData>>(input.HeadersData.Where(t => t.DataType.ToLower() == "header".ToLower()));

            //处理request body
            var request_ids = await _bodyDataManager.QueryAsNoTracking
                .Where(t => t.APIID == agentID && t.DataType.ToLower() == "RequestBody".ToLower())
                .ToListAsync();
            var request_entity = ObjectMapper.Map<List<ApplyBodyData>>(input.RequestData);

            //处理ParameterMapping
            var mapping_ids = _parameterMapping.QueryAsNoTracking.Where(t => t.AgentAPIID == agentID).Select(t => t.Id).ToList();
            var mapping_entity = ObjectMapper.Map<List<ParameterMapping>>(input.ParameterMapping);

            ///事务提交(_applyAPIManager.CreateAsync 指定id ，但是给我刷掉了！！！)
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                ///新增还是修改
                if (isCreate)
                {
                    var myid = await _agentAPIManager.CreateAsync(agentApiEntity);
                    agentID = myid.Id;
                }
                else
                {
                    await _agentAPIManager.UpdateAsync(agentApiEntity);
                }
                //处理query
                foreach (var item in query_entity)
                {
                    item.APIID = agentID;
                    item.Id = Guid.NewGuid();
                    item.IsDeleted = false;
                }
                //处理header
                foreach (var item in header_entity)
                {
                    item.APIID = agentID;
                    item.Id = Guid.NewGuid();
                    item.IsDeleted = false;
                }
                //处理request body
                foreach (var item in request_entity)
                {
                    item.APIID = agentID;
                    item.IsDeleted = false;
                }

                //处理ParameterMapping
                foreach (var item in mapping_entity)
                {
                    item.Id = Guid.NewGuid();
                    item.AgentAPIID = agentID;
                    item.IsDeleted = false;
                }

                await _qhDataManager.BatchDelete(query_ids);
                await _qhDataManager.CreateAsync(query_entity);

                await _qhDataManager.BatchDelete(header_ids);
                await _qhDataManager.CreateAsync(header_entity);

                await _bodyDataManager.DeleteByEntity(request_ids);
                await _bodyDataManager.CreateAsync(request_entity);

                await _parameterMapping.BatchDelete(mapping_ids);
                await _parameterMapping.CreateByEntityAsync(mapping_entity);
                unitOfWork.Complete();
            }
        }

        /// <summary>
        /// 发布和下线
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="Abp.UI.UserFriendlyException"></exception>
        public async Task SetAPIEnableStatus(EntityDto<Guid> input, bool enableStatus)
        {
            var entity = await _agentAPIManager
              .QueryAsNoTracking
              .FirstOrDefaultAsync(x => x.Id == input.Id);

            entity.EnableStatus = enableStatus;

            await _agentAPIManager.UpdateAsync(entity);
        }

        /// <summary>
        /// 查询接口请求记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<APIRequestRecordListDto>> GetAgentAPIRequestRecord(GetAPIRequestRecordPagesInput input)
        {
            var query = _apiLog.Query
                 .WhereIf(input.AgentAPIID != Guid.Empty, a =>
                   a.AgentAPIID == input.AgentAPIID
                ).WhereIf(input.StepBatch != Guid.Empty, a =>
                   a.StepBatch == input.StepBatch
                );

            var count = await query.CountAsync();

            var DataList = await query
                .OrderBy(input.Sorting).AsNoTracking()
                .PageBy(input)
                .ToListAsync();

            var DataListDtos = ObjectMapper.Map<List<APIRequestRecordListDto>>(DataList);

            return new PagedResultDto<APIRequestRecordListDto>(count, DataListDtos);
        }

        /// <summary>
        /// 查询 网关请求记录列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<APIRequestRecordListDto>> GetAgentAPIRequestRecordList(GetAPIRequestRecordPagesInput input)
        {
            //var query = _apiLog.Query
            //            .GroupJoin(_agentAPIManager.Query, log => log.AgentAPIID, ag => ag.Id, (log, ag) => new { log, ag })
            //            .SelectMany(item => item.ag.DefaultIfEmpty(), (item, ag) => new APIRequestRecordListDto
            //            {
            //                AgentName = ag.Name,
            //                AgentPath = ag.Path,
            //                Id = item.log.Id
            //            });

            APIRequestRecordListDto item = new APIRequestRecordListDto();

            var query = _apiLog.Query
                        .Where(t => t.StepType == "接收请求")
                        .Join(_agentAPIManager.Query, log => log.AgentAPIID, ag => ag.Id, (log, ag) => new APIRequestRecordListDto
                        {
                            Id = log.Id,
                            AgentAPIID = ag.Id,
                            AgentName = ag.Name,
                            AgentPath = ag.Path,
                            StepBatch = log.StepBatch,
                            StepName = log.StepName,
                            StepType = log.StepType,
                            StartTime = log.StartTime,
                            StepStatus = (_apiLog.Query.Where(t => t.StepBatch == log.StepBatch).OrderByDescending(t => t.StepSort).FirstOrDefault().StepStatus),
                            StepTime = (_apiLog.Query.Where(t => t.StepBatch == log.StepBatch).Sum(t => Convert.ToDecimal(t.StepTime)).ToString()),
                            StepSort = log.StepSort,
                            CreationTime = log.CreationTime,
                        })
                        .WhereIf(!string.IsNullOrEmpty(input.FilterText),
                        t => t.AgentName.Contains(input.FilterText) ||
                        t.AgentPath.Contains(input.FilterText))
                        .WhereIf(input.StartTime.HasValue, a => a.CreationTime >= input.StartTime)
                        .WhereIf(input.EndTime.HasValue, a => a.CreationTime <= input.EndTime)
                        .OrderByDescending(t => t.StartTime);

            var count = await query.CountAsync();

            var DataList = await query
                .OrderBy(t => t.StartTime).AsNoTracking()
                .PageBy(input)
                .ToListAsync();

            var DataListDtos = ObjectMapper.Map<List<APIRequestRecordListDto>>(query);

            return new PagedResultDto<APIRequestRecordListDto>(count, DataListDtos);
        }

        /// <summary>
        /// 查询基础统计信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Dashboard_TJDto> GetDashboardTJ(Dashboard_TJInput input)
        {
            Dashboard_TJDto _TJDto = new Dashboard_TJDto();

            if (string.IsNullOrWhiteSpace(input.StartTime))
            {
                input.StartTime = DateTime.Now.Date.ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (string.IsNullOrWhiteSpace(input.EndTime))
            {
                input.EndTime = DateTime.Now.Date.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss");
            }

            if (input.dateType != 0)
            {
                if (input.dateType == 1)
                {
                    input.StartTime = DateTime.Now.Date.ToString("yyyy-MM-dd HH:mm:ss");
                    input.EndTime = DateTime.Now.Date.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss");
                }
                if (input.dateType == 7)
                {
                    input.StartTime = DateTime.Now.AddDays(-7).Date.ToString("yyyy-MM-dd HH:mm:ss");
                    input.EndTime = DateTime.Now.Date.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss");
                }
                if (input.dateType == 30)
                {
                    input.StartTime = DateTime.Now.AddDays(-30).Date.ToString("yyyy-MM-dd HH:mm:ss");
                    input.EndTime = DateTime.Now.Date.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss");
                }
            }

            #region API 基础统计信息

            _TJDto.StartTime = Convert.ToDateTime(input.StartTime);
            _TJDto.EndTime = Convert.ToDateTime(input.EndTime);

            //api 总数量
            _TJDto.ApiCounts = await _agentAPIManager.QueryAsNoTracking.CountAsync();

            var apilogs = await _apiLog.QueryAsNoTracking
               .Where(t => t.AgentAPIID != Guid.Empty)
               .Where(t => t.CreationTime >= _TJDto.StartTime && t.CreationTime <= _TJDto.EndTime)
               .ToListAsync();

            //api 请求总次数 RequestApiCounts
            _TJDto.RequestApiCounts = apilogs.Where(t => t.StepType == "接收请求").Count();

            //api 峰值吞吐量 TPS
            int request_count = _TJDto.RequestApiCounts;
            decimal reponsetime = apilogs.Sum(t => t.StepTime.ToDecimal()) / 1000;
            decimal peakThroughput = request_count >= 1 ? request_count / reponsetime : 0;
            _TJDto.PeakThroughput = decimal.Round(peakThroughput, 2);

            //api 请求平均响应时间
            //_TJDto.AvgResponseTime = apilogs.Select(t => t.StepTime.ToDecimal()).Count() > 0 ? apilogs.Select(t => t.StepTime.ToDecimal()).Average() : 0;
            //_TJDto.AvgResponseTime = decimal.Round(_TJDto.AvgResponseTime, 2);

            //api 异常数量
            foreach (var item in apilogs.Where(t => t.StepType == "接收请求"))
            {
                var istrue = apilogs.Where(t => t.AgentAPIID == item.AgentAPIID && t.StepBatch == item.StepBatch)
                    .OrderByDescending(t => t.StartTime).FirstOrDefault().StepStatus;
                if (!istrue)
                {
                    _TJDto.ExceptionCounts += 1;
                }
            }
            //api告警数量
            _TJDto.ReportEmergencyCounts = 0;

            #endregion API 基础统计信息

            TimeSpan timeDifference = Convert.ToDateTime(_TJDto.EndTime) - Convert.ToDateTime(_TJDto.StartTime);

            if (timeDifference.Days < 7)
            {
                #region API 流量趋势 & 网关平均响应时间

                List<string> xAxis_data = new List<string>();
                List<int> series_all = new List<int>();
                List<int> series_success = new List<int>();
                List<int> series_error = new List<int>();

                List<decimal> reponse_all = new List<decimal>();
                List<decimal> reponse_wg = new List<decimal>();
                List<decimal> reponse_fw = new List<decimal>();

                //查询时间区间 小时数量
                //TimeSpan timeDifference = Convert.ToDateTime(_TJDto.EndTime) - Convert.ToDateTime(_TJDto.StartTime);
                int hoursDifference = Convert.ToInt32(timeDifference.TotalHours);
                int starthours = Convert.ToDateTime(_TJDto.StartTime).Hour;
                DateTime startdate = Convert.ToDateTime(_TJDto.StartTime);

                decimal avg_response_time = 0.00m;

                for (int i = 0; i < hoursDifference; i++)
                {
                    // x 轴数据
                    xAxis_data.Add($"{starthours.ToString().PadLeft(2, '0')}:00");

                    //这个小时内 有多少请求
                    var logs = apilogs.Where(t => t.CreationTime >= startdate && t.CreationTime <= startdate.AddHours(1).AddSeconds(-1))
                        .Where(t => t.StepType == "接收请求");

                    series_all.Add(logs.Count());

                    //这个小时内 的请求 成功有多少 失败有多少   以及响应时间
                    int success_1 = 0;
                    int error_1 = 0;

                    //这个接口 请求总耗时  网关请求耗时 后台服务请求耗时
                    decimal all_time = 0.00m;
                    decimal wg_time = 0.00m;
                    decimal fw_time = 0.00m;
                    foreach (var log in logs)
                    {
                        var istrue = apilogs.Where(t => t.AgentAPIID == log.AgentAPIID && t.StepBatch == log.StepBatch)
                                    .OrderByDescending(t => t.StartTime).FirstOrDefault().StepStatus;
                        if (istrue)
                        {
                            success_1++;
                        }
                        else
                        {
                            error_1++;
                        }

                        //请求对应的   所有步骤的响应时间
                        var reponsestime = apilogs.Where(t => t.AgentAPIID == log.AgentAPIID && t.StepBatch == log.StepBatch)
                                 .OrderByDescending(t => t.StartTime);

                        var is_wg_time = reponsestime.Where(t => t.StepType != "请求后台服务").Select(t => t.StepTime.ToDecimal());
                        var is_fw_time = reponsestime.Where(t => t.StepType == "请求后台服务").Select(t => t.StepTime.ToDecimal());

                        wg_time += is_wg_time.Count() > 0 ? is_wg_time.Average() : 0.00m;
                        fw_time += is_fw_time.Count() > 0 ? is_fw_time.Average() : 0.00m;
                    }
                    all_time += wg_time + fw_time;
                    avg_response_time += all_time;
                    series_success.Add(success_1);
                    series_error.Add(error_1);

                    reponse_all.Add(decimal.Round(all_time, 2));
                    reponse_wg.Add(decimal.Round(wg_time, 2));
                    reponse_fw.Add(decimal.Round(fw_time, 2));

                    //自增设置
                    starthours++;
                    startdate = startdate.AddHours(1);
                    if (starthours >= 24)
                    {
                        starthours = 0;
                    }
                }
                //接口请求平均耗时=接口请求总耗时/接口请求次数
                if (avg_response_time > 0)
                {
                    _TJDto.AvgResponseTime = decimal.Round(avg_response_time / _TJDto.RequestApiCounts, 2);
                }
                else
                {
                    _TJDto.AvgResponseTime = 0.00m;
                }
                _TJDto.volume_xAxis_data = xAxis_data.ToArray().ToJsonString();
                _TJDto.volume_series_data0 = series_all.ToArray().ToJsonString();
                _TJDto.volume_series_data1 = series_success.ToArray().ToJsonString();
                _TJDto.volume_series_data2 = series_error.ToArray().ToJsonString();

                _TJDto.reponse_series_data0 = reponse_all.ToArray().ToJsonString();
                _TJDto.reponse_series_data1 = reponse_wg.ToArray().ToJsonString();
                _TJDto.reponse_series_data2 = reponse_fw.ToArray().ToJsonString();

                #endregion API 流量趋势 & 网关平均响应时间
            }
            if (timeDifference.Days >= 7 && timeDifference.Days <= 30)
            {
                #region API 流量趋势 & 网关平均响应时间

                List<string> xAxis_data = new List<string>();
                List<int> series_all = new List<int>();
                List<int> series_success = new List<int>();
                List<int> series_error = new List<int>();

                List<decimal> reponse_all = new List<decimal>();
                List<decimal> reponse_wg = new List<decimal>();
                List<decimal> reponse_fw = new List<decimal>();

                //查询时间区间 日期数量
                int daysDifference = Convert.ToInt32(timeDifference.TotalDays);
                DateTime startdate = Convert.ToDateTime(_TJDto.StartTime).Date;
                decimal avg_response_time = 0.00m;

                for (int i = 0; i < daysDifference; i++)
                {
                    // x 轴数据
                    xAxis_data.Add($"{startdate.ToString("yyyy-MM-dd")}");

                    //这个小时内 有多少请求
                    var logs = apilogs.Where(t => t.CreationTime >= startdate && t.CreationTime <= startdate.AddDays(1).AddSeconds(-1))
                        .Where(t => t.StepType == "接收请求");

                    series_all.Add(logs.Count());

                    //这个小时内 的请求 成功有多少 失败有多少   以及响应时间
                    int success_1 = 0;
                    int error_1 = 0;

                    //这个接口 请求总耗时  网关请求耗时 后台服务请求耗时
                    decimal all_time = 0.00m;
                    decimal wg_time = 0.00m;
                    decimal fw_time = 0.00m;
                    foreach (var log in logs)
                    {
                        var istrue = apilogs.Where(t => t.AgentAPIID == log.AgentAPIID && t.StepBatch == log.StepBatch)
                                    .OrderByDescending(t => t.StartTime).FirstOrDefault().StepStatus;
                        if (istrue)
                        {
                            success_1++;
                        }
                        else
                        {
                            error_1++;
                        }

                        //请求对应的   所有步骤的响应时间
                        var reponsestime = apilogs.Where(t => t.AgentAPIID == log.AgentAPIID && t.StepBatch == log.StepBatch)
                                 .OrderByDescending(t => t.StartTime);

                        var is_wg_time = reponsestime.Where(t => t.StepType != "请求后台服务").Select(t => t.StepTime.ToDecimal());
                        var is_fw_time = reponsestime.Where(t => t.StepType == "请求后台服务").Select(t => t.StepTime.ToDecimal());

                        wg_time += is_wg_time.Count() > 0 ? is_wg_time.Average() : 0.00m;
                        fw_time += is_fw_time.Count() > 0 ? is_fw_time.Average() : 0.00m;
                    }
                    all_time += wg_time + fw_time;
                    avg_response_time += all_time;
                    series_success.Add(success_1);
                    series_error.Add(error_1);

                    reponse_all.Add(decimal.Round(all_time, 2));
                    reponse_wg.Add(decimal.Round(wg_time, 2));
                    reponse_fw.Add(decimal.Round(fw_time, 2));

                    //自增设置
                    startdate = startdate.AddDays(1);
                }
                //接口请求平均耗时=接口请求总耗时/接口请求次数
                if (avg_response_time > 0)
                {
                    _TJDto.AvgResponseTime = decimal.Round(avg_response_time / _TJDto.RequestApiCounts, 2);
                }
                else
                {
                    _TJDto.AvgResponseTime = 0.00m;
                }
                _TJDto.volume_xAxis_data = xAxis_data.ToArray().ToJsonString();
                _TJDto.volume_series_data0 = series_all.ToArray().ToJsonString();
                _TJDto.volume_series_data1 = series_success.ToArray().ToJsonString();
                _TJDto.volume_series_data2 = series_error.ToArray().ToJsonString();

                _TJDto.reponse_series_data0 = reponse_all.ToArray().ToJsonString();
                _TJDto.reponse_series_data1 = reponse_wg.ToArray().ToJsonString();
                _TJDto.reponse_series_data2 = reponse_fw.ToArray().ToJsonString();

                #endregion API 流量趋势 & 网关平均响应时间
            }

            #region API接口列表相关统计

            var agentList = await _agentAPIManager.QueryAsNoTracking.Where(t => apilogs.Select(t => t.AgentAPIID).Contains(t.Id)).ToListAsync();

            List<Dashboard_apiListDto> apiLists = new List<Dashboard_apiListDto>();
            foreach (var agent in agentList)
            {
                Dashboard_apiListDto apiList = new Dashboard_apiListDto();
                apiList.id = agent.Id;
                apiList.apiName = agent.Name;
                apiList.apiPaht = agent.Path;

                var request_log = apilogs.Where(t => t.AgentAPIID == agent.Id && t.StepType == "接收请求");
                apiList.requestCounts = request_log.Count();

                decimal api_error = 0;
                decimal api_wg_time = 0.00m;
                decimal api_fw_time = 0.00m;
                foreach (var item in request_log)
                {
                    var istrue = apilogs.Where(t => t.AgentAPIID == item.AgentAPIID && t.StepBatch == item.StepBatch)
                             .OrderByDescending(t => t.StartTime).FirstOrDefault().StepStatus;
                    if (!istrue)
                    {
                        api_error++;
                    }
                    //请求对应的   所有步骤的响应时间
                    var reponsestime = apilogs.Where(t => t.AgentAPIID == item.AgentAPIID && t.StepBatch == item.StepBatch)
                             .OrderByDescending(t => t.StartTime);

                    var is_wg_time = reponsestime.Where(t => t.StepType != "请求后台服务").Select(t => t.StepTime.ToDecimal());
                    var is_fw_time = reponsestime.Where(t => t.StepType == "请求后台服务").Select(t => t.StepTime.ToDecimal());

                    api_wg_time += is_wg_time.Count() > 0 ? is_wg_time.Average() : 0.00m;
                    api_fw_time += is_fw_time.Count() > 0 ? is_fw_time.Average() : 0.00m;
                }

                apiList.errorCounts = api_error;
                apiList.errorRate = decimal.Round(api_error / apiList.requestCounts * 100, 2);
                apiList.wgResponse = decimal.Round(api_wg_time, 2);
                apiList.fwResponse = decimal.Round(api_fw_time, 2);

                decimal api_reponsetime = apilogs.Sum(t => t.StepTime.ToDecimal()) / 1000;
                apiList.peakThroughput = decimal.Round((apiList.requestCounts / api_reponsetime), 2);

                apiLists.Add(apiList);
            }
            _TJDto.apiList = apiLists.OrderByDescending(t => t.errorCounts).ToList();

            #endregion API接口列表相关统计

            return _TJDto;
        }
    }
}
