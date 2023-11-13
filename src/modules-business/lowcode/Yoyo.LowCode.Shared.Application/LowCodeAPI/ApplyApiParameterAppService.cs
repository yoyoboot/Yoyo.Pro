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
using Microsoft.EntityFrameworkCore;
using Yoyo.LowCode.LowCodeAPI.DomainService;
using Yoyo.LowCode.LowCodeAPI.Dtos.ApplyApiParameterDto;

namespace Yoyo.LowCode.LowCodeAPI
{
    public class ApplyApiParameterAppService : LowCodeSharedAppServiceBase, IApplyApiParameterAppService
    {
        #region Private Fields

        private readonly IUnitOfWorkManager _unitOfWorkManager;

        private readonly IApplyBodyDataManager _bodyDataManager;
        private readonly IApplyQHDataManager _qhDataManager;
        private readonly IParameterMappingManager _parameterMapping;

        #endregion Private Fields

        #region Public Constructors

        public ApplyApiParameterAppService(IApplyBodyDataManager applyBodyData, IApplyQHDataManager applyQHData
            , IUnitOfWorkManager unitOfWorkManager, IParameterMappingManager parameterMapping)
        {
            _bodyDataManager = applyBodyData;
            _qhDataManager = applyQHData;
            _unitOfWorkManager = unitOfWorkManager;
            _parameterMapping = parameterMapping;
        }

        #endregion Public Constructors

        public async Task BatchDeleteForApplyBodyData(List<Guid> input)
        {
            await _bodyDataManager.BatchDelete(input);
        }

        public async Task BatchDeleteForApplyQHData(List<Guid> input)
        {
            await _qhDataManager.BatchDelete(input);
        }

        /// <summary>
        /// 添加或者修改 API接口参数ApplyBodyData的配置 先删后插入
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdateForApplyBodyData(CreateOrUpdateApplyBodyData input)
        {
            var applyApiID = input.ApplyBodyDataEditDto.Select(t => t.APIID).FirstOrDefault();
            var oldData = await _bodyDataManager.QueryAsNoTracking.Where(t => t.APIID == applyApiID).ToListAsync();

            var entity = ObjectMapper.Map<List<ApplyBodyData>>(input.ApplyBodyDataEditDto);
            foreach (var item in entity)
            {
                item.IsDeleted = false;
            }
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                await _bodyDataManager.DeleteByEntity(oldData);
                await _bodyDataManager.CreateAsync(entity);
                unitOfWork.Complete();
            }
        }

        /// <summary>
        /// 添加或者修改 API接口参数ApplyBodyData的配置 先删后插入
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdateForApplyQHData(CreateOrUpdateApplyQHData input)
        {
            var apiID = input.ApplyQHDataEditDto.Select(t => t.APIID).FirstOrDefault();
            var ids = _qhDataManager.QueryAsNoTracking.Where(t => t.APIID == apiID).Select(t => t.Id).ToList();

            var entity = ObjectMapper.Map<List<ApplyQHData>>(input.ApplyQHDataEditDto);
            foreach (var item in entity)
            {
                item.Id = Guid.NewGuid();
                item.IsDeleted = false;
            }
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                await _qhDataManager.BatchDelete(ids);
                await _qhDataManager.CreateAsync(entity);
                unitOfWork.Complete();
            }
        }

        /// <summary>
        /// 获取应用下API接口参数ApplyBodyData 列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PagedResultDto<ApplyBodyDataListDto>> GetApplyBodyDataList(GetPublicPagesInput input)
        {
            var query = _bodyDataManager.Query
               .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a =>
                 a.FileName.Contains(input.FilterText)
             ).WhereIf(input.APIID != Guid.Empty, a =>
                a.APIID == input.APIID
            );

            var count = await query.CountAsync();

            var DataList = await query
                .OrderBy(input.Sorting).AsNoTracking()
                .PageBy(input)
                .ToListAsync();

            var DataListDtos = ObjectMapper.Map<List<ApplyBodyDataListDto>>(DataList);

            return new PagedResultDto<ApplyBodyDataListDto>(count, DataListDtos);
        }

        /// <summary>
        /// 获取应用下API接口参数ApplyQHData配置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PagedResultDto<ApplyQHDataListDto>> GetApplyQHDataDataList(GetPublicPagesInput input)
        {
            var query = _qhDataManager.Query
              .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a =>
                a.FileName.Contains(input.FilterText)
            ).WhereIf(input.APIID != Guid.Empty, a =>
               a.APIID == input.APIID
           );

            var count = await query.CountAsync();

            var DataList = await query
                .OrderBy(input.Sorting).AsNoTracking()
                .PageBy(input)
                .ToListAsync();

            var DataListDtos = ObjectMapper.Map<List<ApplyQHDataListDto>>(DataList);

            return new PagedResultDto<ApplyQHDataListDto>(count, DataListDtos);
        }

        /// <summary>
        /// 获取API Query数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<ApplyQHDataListDto>> GetApiQueryData(EntityDto<Guid> input)
        {
            var entity = await _qhDataManager.QueryAsNoTracking.Where(t => t.APIID == input.Id
            && t.DataType.ToLower() == "Query".ToLower()).ToListAsync();
            var dto = ObjectMapper.Map<List<ApplyQHDataListDto>>(entity);
            return dto;
        }

        /// <summary>
        /// 获取API Header数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<ApplyQHDataListDto>> GetApiHeaderData(EntityDto<Guid> input)
        {
            var entity = await _qhDataManager.QueryAsNoTracking.Where(t => t.APIID == input.Id
            && t.DataType.ToLower() == "Header".ToLower()).ToListAsync();
            var dto = ObjectMapper.Map<List<ApplyQHDataListDto>>(entity);
            return dto;
        }

        /// <summary>
        /// 获取API的RequestBody参数信息
        /// </summary>
        /// <param name="input">API ID</param>
        /// <param name="isPackage">是否返回组装后的数据</param>
        /// <returns></returns>
        public async Task<List<ApplyBodyDataListDto>> GetApiRequestBodyData(EntityDto<Guid> input, bool isPackage)
        {
            var query = await _bodyDataManager.QueryAsNoTracking.Where(t => t.APIID == input.Id
            && t.DataType.ToLower() == "RequestBody".ToLower()).ToListAsync();
            var entity = ObjectMapper.Map<List<ApplyBodyDataListDto>>(query);
            if (!isPackage)
            {
                return entity;
            }
            //组装数据
            List<ApplyBodyDataListDto> RequestBody = new List<ApplyBodyDataListDto>();
            var datas = entity.Where(t => t.ParentID == Guid.Empty).FirstOrDefault();
            if (datas != null)
            {
                datas.Children = FindChildren(entity, null);
                RequestBody.Add(datas);
            }
            return RequestBody;
        }

        /// <summary>
        /// 获取API的ResponseBody参数信息
        /// </summary>
        /// <param name="input">API ID</param>
        /// <param name="isPackage">是否返回组装后的数据</param>
        /// <returns></returns>
        public async Task<List<ApplyBodyDataListDto>> GetApiResponseBodyData(EntityDto<Guid> input, bool isPackage)
        {
            var query = await _bodyDataManager.QueryAsNoTracking.Where(t => t.APIID == input.Id
            && t.DataType.ToLower() == "ResponseBody".ToLower()).ToListAsync();
            var entity = ObjectMapper.Map<List<ApplyBodyDataListDto>>(query);
            if (!isPackage)
            {
                return entity;
            }
            //组装数据
            List<ApplyBodyDataListDto> RequestBody = new List<ApplyBodyDataListDto>();
            var datas = entity.Where(t => t.ParentID == Guid.Empty).FirstOrDefault();
            if (datas != null)
            {
                datas.Children = FindChildren(entity, null);
                RequestBody.Add(datas);
            }
            return RequestBody;
        }

        /// <summary>
        /// 组装数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="recordId"></param>
        /// <returns></returns>
        internal List<ApplyBodyDataListDto> FindChildren(List<ApplyBodyDataListDto> data, Guid? recordId)
        {
            if (data.Count <= 0)
            {
                return null;
            }
            List<ApplyBodyDataListDto> treeList = new List<ApplyBodyDataListDto>();
            recordId = recordId == null ? data.Where(t => t.ParentID == Guid.Empty).FirstOrDefault().Id : recordId;
            //获取id的所有下级
            List<ApplyBodyDataListDto> dataList = data.Where(u => u.ParentID == recordId).ToList();
            foreach (var item in dataList)
            {
                ApplyBodyDataListDto model = new ApplyBodyDataListDto();
                model.key = item.Id.ToString();
                model.Id = item.Id;
                model.APIID = item.APIID;
                model.DataType = item.DataType;
                model.ParentID = item.ParentID;
                model.FileName = item.FileName;
                model.FileType = item.FileType;
                model.IsRequired = item.IsRequired;
                model.Description = item.Description;
                model.MaxLength = item.MaxLength;
                model.DefaultValue = item.DefaultValue;
                //循环调用，获取当前的子目录
                model.Children = FindChildren(data, item.Id);
                treeList.Add(model);
            }
            return treeList.Count() > 0 ? treeList : null;
        }

        /// <summary>
        /// 新增修改 API参数Mapping关系
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdateForParameterMapping(CreateOrUpdateParameterMapping input)
        {
            var apiID = input.ParameterMappingEditDto.Select(t => t.AgentAPIID).FirstOrDefault();
            var ids = _parameterMapping.QueryAsNoTracking.Where(t => t.AgentAPIID == apiID).Select(t => t.Id).ToList();

            var entity = ObjectMapper.Map<List<ParameterMapping>>(input.ParameterMappingEditDto);
            foreach (var item in entity)
            {
                item.Id = Guid.NewGuid();
                item.IsDeleted = false;
            }
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                await _parameterMapping.BatchDelete(ids);
                await _parameterMapping.CreateByEntityAsync(entity);
                unitOfWork.Complete();
            }
        }

        /// <summary>
        /// 批量删除  API参数Mapping关系
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task BatchDeleteForParameterMapping(List<Guid> input)
        {
            await _parameterMapping.BatchDelete(input);
        }

        public async Task<PagedResultDto<ParameterMappingListDto>> GetParameterMappingList(GetPublicPagesInput input)
        {
            var query = _parameterMapping.Query.WhereIf(input.AgentAPIID != Guid.Empty, a =>
                a.AgentAPIID == input.AgentAPIID
            );

            var count = await query.CountAsync();

            var DataList = await query
                .OrderBy(input.Sorting).AsNoTracking()
                .PageBy(input)
                .ToListAsync();

            var DataListDtos = ObjectMapper.Map<List<ParameterMappingListDto>>(DataList);

            return new PagedResultDto<ParameterMappingListDto>(count, DataListDtos);
        }
    }
}
