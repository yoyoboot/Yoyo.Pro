// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net.Http;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using NSwag;
using Yoyo.LowCode.LowCodeAPI.DomainService;
using Yoyo.LowCode.LowCodeAPI.Dtos.ApplyApiDto;
using Yoyo.LowCode.LowCodeAPI.Dtos.ApplyApiParameterDto;

namespace Yoyo.LowCode.LowCodeAPI
{
    public class ApplyAPIAppService : LowCodeSharedAppServiceBase, IApplyAPIAppService
    {
        #region Private Fields

        private readonly IUnitOfWorkManager _unitOfWorkManager;

        private readonly IApplyAPIManager _applyAPIManager;

        private readonly IApplyBodyDataManager _bodyDataManager;
        private readonly IApplyQHDataManager _qhDataManager;

        #endregion Private Fields

        #region Public Constructors

        public ApplyAPIAppService(IApplyAPIManager applyAPIManager
            , IApplyBodyDataManager bodyDataManager
            , IApplyQHDataManager qHDataManager
            , IUnitOfWorkManager unitOfWorkManager)
        {
            _applyAPIManager = applyAPIManager;
            _bodyDataManager = bodyDataManager;
            _qhDataManager = qHDataManager;
            _unitOfWorkManager = unitOfWorkManager;
        }

        #endregion Public Constructors

        /// <summary>
        /// 根据ID 批量删除API接口数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task BatchDelete(List<Guid> input)
        {
            await _applyAPIManager.BatchDelete(input);
        }

        /// <summary>
        /// 新增或者修改API接口数据配置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task CreateOrUpdate(CreateOrUpdateApplyAPI input)
        {
            var applyTypeList = await _applyAPIManager.QueryAsNoTracking.Where(t => t.Id != input.ApplyAPIEditDto.Id && t.ApplyID == input.ApplyAPIEditDto.ApplyID).ToListAsync();

            if (applyTypeList.Any(t => t.Name == input.ApplyAPIEditDto.Name))
            {
                throw new Abp.UI.UserFriendlyException(L($"当前应用下API名称{input.ApplyAPIEditDto.Name}已被使用，请重新输入名称"));
            }

            var entity = ObjectMapper.Map<ApplyAPI>(input.ApplyAPIEditDto);
            ///新增还是修改
            if (input.ApplyAPIEditDto.Id.HasValue)
            {
                await _applyAPIManager.UpdateAsync(entity);
            }
            else
            {
                await _applyAPIManager.CreateAsync(entity);
            }
        }

        /// <summary>
        /// 根据ID 删除API接口数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task Delete(EntityDto<Guid> input)
        {
            await _applyAPIManager.DeleteAsync(input.Id);
        }

        /// <summary>
        /// 获取API接口数据 分页(根据网关应用ID)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PagedResultDto<ApplyAPIListDto>> GetApplyAPIListByApplyID(GetApplyAPIPagesInput input)
        {
            var query = _applyAPIManager.Query
                 .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a =>
                   a.Name.Contains(input.FilterText) ||
                   a.Path.Contains(input.FilterText)
               ).WhereIf(input.ApplyID != Guid.Empty, a =>
                  a.ApplyID == input.ApplyID
              );

            var count = await query.CountAsync();

            var DataList = await query
                .OrderBy(input.Sorting).AsNoTracking()
                .PageBy(input)
                .ToListAsync();

            var DataListDtos = ObjectMapper.Map<List<ApplyAPIListDto>>(DataList);

            return new PagedResultDto<ApplyAPIListDto>(count, DataListDtos);
        }

        /// <summary>
        /// 根据ID获取API接口数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApplyAPIListDto> GetById(EntityDto<Guid> input)
        {
            var entity = await _applyAPIManager
            .QueryAsNoTracking
            .FirstOrDefaultAsync(x => x.Id == input.Id);

            var dto = ObjectMapper.Map<ApplyAPIListDto>(entity);
            return dto;
        }

        /// <summary>
        /// 新增修改连接应用下API配置数据（api信息 query header requestbody responsebody）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="Abp.UI.UserFriendlyException"></exception>
        public async Task CreateOrUpdateAPISetting(CreateOrUpdateApplyAPI input)
        {
            bool isCreate = false;
            //处理apply  api
            var applyAPIList = await _applyAPIManager.QueryAsNoTracking.Where(t => t.Id != input.ApplyAPIEditDto.Id).ToListAsync();
            Guid apiID = Guid.Empty;
            if (applyAPIList.Any(t => t.Name == input.ApplyAPIEditDto.Name && t.ApplyID == input.ApplyAPIEditDto.ApplyID))
            {
                throw new Abp.UI.UserFriendlyException(L($"当前应用下API名称{input.ApplyAPIEditDto.Name}已被使用，请重新输入名称"));
            }
            if (applyAPIList.Any(t => t.Path == input.ApplyAPIEditDto.Path && t.ApplyID == input.ApplyAPIEditDto.ApplyID))
            {
                throw new Abp.UI.UserFriendlyException(L($"当前应用下API请求路径{input.ApplyAPIEditDto.Path}已被使用，请重新输入请求路径"));
            }
            var entity = ObjectMapper.Map<ApplyAPI>(input.ApplyAPIEditDto);
            if (input.ApplyAPIEditDto.Id.HasValue)
            {
                apiID = (Guid)input.ApplyAPIEditDto.Id;
                isCreate = false;
            }
            else
            {
                isCreate = true;
                apiID = Guid.NewGuid();
                input.ApplyAPIEditDto.Id = apiID;
            }

            ///处理query
            var query_ids = _qhDataManager.QueryAsNoTracking
                .Where(t => t.APIID == apiID && t.DataType.ToLower() == "query".ToLower())
                .Select(t => t.Id).ToList();
            var query_entity = ObjectMapper.Map<List<ApplyQHData>>(input.QueryData);

            ///处理header
            var header_ids = _qhDataManager.QueryAsNoTracking
              .Where(t => t.APIID == apiID && t.DataType.ToLower() == "Header".ToLower())
              .Select(t => t.Id).ToList();
            var header_entity = ObjectMapper.Map<List<ApplyQHData>>(input.HeadersData);

            //处理request body
            var request_ids = await _bodyDataManager.QueryAsNoTracking
                .Where(t => t.APIID == apiID && t.DataType.ToLower() == "RequestBody".ToLower())
                .ToListAsync();
            var request_entity = ObjectMapper.Map<List<ApplyBodyData>>(input.RequestData);

            //处理reponse body
            var reponse_ids = await _bodyDataManager.QueryAsNoTracking
                .Where(t => t.APIID == apiID && t.DataType.ToLower() == "ResponseBody".ToLower())
                .ToListAsync();
            var reponse_entity = ObjectMapper.Map<List<ApplyBodyData>>(input.ReponseData);

            ///事务提交(_applyAPIManager.CreateAsync 指定id ，但是给我刷掉了！！！)
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                ///新增还是修改
                if (isCreate)
                {
                    var myid = await _applyAPIManager.CreateAsync(entity);
                    apiID = myid.Id;
                }
                else
                {
                    await _applyAPIManager.UpdateAsync(entity);
                }
                //处理query
                foreach (var item in query_entity)
                {
                    item.APIID = apiID;
                    item.Id = Guid.NewGuid();
                    item.IsDeleted = false;
                }
                //处理header
                foreach (var item in header_entity)
                {
                    item.APIID = apiID;
                    item.Id = Guid.NewGuid();
                    item.IsDeleted = false;
                }
                //处理request body
                foreach (var item in request_entity)
                {
                    item.APIID = apiID;
                    item.IsDeleted = false;
                }

                //处理reponse body
                foreach (var item in reponse_entity)
                {
                    item.APIID = apiID;
                    item.IsDeleted = false;
                }

                await _qhDataManager.BatchDelete(query_ids);
                await _qhDataManager.CreateAsync(query_entity);

                await _qhDataManager.BatchDelete(header_ids);
                await _qhDataManager.CreateAsync(header_entity);

                await _bodyDataManager.DeleteByEntity(request_ids);
                await _bodyDataManager.CreateAsync(request_entity);

                await _bodyDataManager.DeleteByEntity(reponse_ids);
                await _bodyDataManager.CreateAsync(reponse_entity);
                unitOfWork.Complete();
            }
        }

        /// <summary>
        /// 导入swagger文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="applyID"></param>
        /// <returns></returns>
        public async Task<List<ImprotSwaggerData>> ImprotSwagger(string url, Guid applyID)
        {
            List<ImprotSwaggerData> improtApplyAPI = new List<ImprotSwaggerData>();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var uriBuilder = new UriBuilder(url);
                    var request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.ToString());
                    var response = await httpClient.SendAsync(request);
                    var responseData = await response.Content.ReadAsStringAsync();
                    // 处理 $ref 内的循环引用
                    var swaggerJToken = JToken.Parse(responseData);
                    RemoveCircularReferences(swaggerJToken["components"]?["schemas"]);
                    //解析成SwaggerDocument
                    var swaggerJson = SwaggerDocument.FromJsonAsync(swaggerJToken.ToString()).Result;
                    //当前appyid下已经存在的api信息

                    var OldappluApiList = await _applyAPIManager.QueryAsNoTracking
                                        .Where(t => t.ApplyID == applyID)
                                        .ToListAsync();
                    //int icount = 0;
                    //处理数据
                    foreach (var pathItem in swaggerJson.Paths)
                    {
                        //icount++;
                        //if (icount >= 20)
                        //{
                        //    break;
                        //}

                        //if (pathItem.Key != "/api/services/app/UserEmailer/SendEmailActivationLink")
                        //{
                        //    break;
                        //}

                        ImprotSwaggerData applyAPIList = new ImprotSwaggerData();
                        List<ApplyQHDataListDto> qHDataListDtos = new List<ApplyQHDataListDto>();
                        applyAPIList.Id = Guid.NewGuid();
                        applyAPIList.ApplyID = applyID;
                        foreach (var operation in pathItem.Value)
                        {
                            applyAPIList.DataSource = "数据导入";
                            applyAPIList.Name = string.IsNullOrEmpty(operation.Value.Summary) ? pathItem.Key : operation.Value.Summary;
                            applyAPIList.MethodName = operation.Key.ToString();
                            applyAPIList.MethodCode = operation.Key.ToString();
                            applyAPIList.Path = pathItem.Key;
                            applyAPIList.IsRepeat = false;
                            //判断是否重复
                            if (OldappluApiList.Where(t => t.Path == applyAPIList.Path).Count() > 0)
                            {
                                applyAPIList.IsRepeat = true;
                            }
                            applyAPIList.LableName = "";
                            applyAPIList.LableCode = "";
                            applyAPIList.Remarks = "";

                            applyAPIList.QueryJson = "";
                            applyAPIList.HeadersJson = "";
                            applyAPIList.RequestBodyDataTypeName = "application/json";
                            applyAPIList.RequestBodyDataTypeCode = "application/json";

                            applyAPIList.ResponseBodyDataTypeName = "application/json";
                            applyAPIList.ResponseBodyDataTypeCode = "application/json";
                            applyAPIList.ResponseBodyJson = "";
                            applyAPIList.requestBodyList = new List<ApplyBodyDataListDto>();
                            applyAPIList.responseBodyList = new List<ApplyBodyDataListDto>();
                            //获取query  header传参
                            foreach (var parameter in operation.Value.Parameters.Where(t => t.Kind.ToString().ToLower() != "Body".ToLower()))
                            {
                                ApplyQHDataListDto ApplyQHData = new ApplyQHDataListDto();
                                ApplyQHData.DataSource = "数据导入";
                                ApplyQHData.Id = Guid.NewGuid();
                                ApplyQHData.APIID = applyAPIList.Id;
                                ApplyQHData.DataType = parameter.Kind.ToString();
                                ApplyQHData.FileName = parameter.Name;
                                ApplyQHData.FileType = parameter.Schema.Type.ToString() == "None" ? "string" : parameter.Schema.Type.ToString();
                                ApplyQHData.IsRequired = parameter.IsRequired;
                                ApplyQHData.Description = parameter.Description;
                                ApplyQHData.MaxLength = 0;
                                ApplyQHData.DefaultValue = "";
                                ApplyQHData.ReceivedValue = "";

                                qHDataListDtos.Add(ApplyQHData);
                            }

                            ///请求body参数
                            if (operation.Value.RequestBody != null)
                            {
                                //request 请求传参 内容
                                var requestContent = operation.Value.RequestBody.Content.FirstOrDefault();
                                if (requestContent.Key != null && requestContent.Value != null)
                                {
                                    ApplyBodyDataListDto requestBody = new ApplyBodyDataListDto();
                                    requestBody.DataSource = "数据导入";
                                    requestBody.Id = Guid.NewGuid();
                                    requestBody.key = requestBody.Id.ToString();
                                    requestBody.APIID = applyAPIList.Id;
                                    requestBody.DataType = "RequestBody";
                                    requestBody.ParentID = Guid.Empty;
                                    requestBody.FileName = "RequestBody";
                                    requestBody.FileType = "object";
                                    requestBody.IsRequired = true;
                                    requestBody.Description = "";
                                    requestBody.MaxLength = 0;
                                    requestBody.DefaultValue = "";
                                    requestBody.ReceivedValue = "";
                                    requestBody.Path = "RequestBody";

                                    //request 请求传参 类型
                                    var requestType = requestContent.Value?.Schema?.Type.ToString();
                                    requestType = requestType == null ? "null" : requestType;
                                    if (requestType.ToLower() == "None".ToLower())
                                    {
                                        //request 请求传参 映射的实体
                                        if (requestContent.Value.Schema.Reference != null)
                                        {
                                            foreach (var reqitem in requestContent.Value.Schema.Reference.ActualProperties)
                                            {
                                                ApplyBodyDataListDto rB_item = new ApplyBodyDataListDto();
                                                rB_item.DataSource = "数据导入";
                                                rB_item.Id = Guid.NewGuid();
                                                rB_item.key = rB_item.Id.ToString();
                                                rB_item.APIID = applyAPIList.Id;
                                                rB_item.DataType = "RequestBody";
                                                rB_item.ParentID = requestBody.Id;
                                                rB_item.FileName = reqitem.Key;
                                                rB_item.FileType = reqitem.Value.Type.ToString() == "None" ? "object" : reqitem.Value.Type.ToString();
                                                rB_item.IsRequired = reqitem.Value.IsRequired;
                                                rB_item.Description = reqitem.Value.Description;
                                                rB_item.MaxLength = 0;
                                                rB_item.DefaultValue = "";
                                                rB_item.ReceivedValue = "";
                                                rB_item.Path = "";

                                                if (rB_item.FileType.ToLower() == "Array".ToLower())
                                                {
                                                    var type_rB_item = reqitem.Value.Item?.ActualSchema?.Type.ToString() == "None"
                                                                         ? "arrayOfString"
                                                                         : "arrayOf" + reqitem.Value.Item?.ActualSchema?.Type.ToString();
                                                    rB_item.FileType = type_rB_item;
                                                }

                                                List<ApplyBodyDataListDto> applies = new List<ApplyBodyDataListDto>();

                                                //入参对象类型找不到有问题
                                                findChilderLeve(reqitem, applies, applyAPIList, rB_item, 0);

                                                if (applies.Count > 0)
                                                {
                                                    rB_item.Children = applies;
                                                }

                                                //过滤掉IsNullableRaw==null的数据
                                                if (reqitem.Value.IsNullableRaw != null)
                                                {
                                                    if (requestBody.Children == null)
                                                    {
                                                        requestBody.Children = new List<ApplyBodyDataListDto>();
                                                    }
                                                    requestBody.Children.Add(rB_item);
                                                }
                                            }
                                        }
                                    }
                                    else if (requestType.ToLower() == "array".ToLower())
                                    {
                                        var type = requestContent.Value.Schema?.Item?.ActualSchema?.Type.ToString() == "None"
                                                       ? "arrayOfString"
                                                       : "arrayOf" + requestContent.Value.Schema?.Item?.ActualSchema?.Type.ToString();
                                        requestBody.FileType = type;

                                        foreach (var item_lv in requestContent.Value.Schema.Item.ActualSchema.ActualProperties)
                                        {
                                            ApplyBodyDataListDto rb2_item = new ApplyBodyDataListDto();
                                            rb2_item.DataSource = "数据导入";
                                            rb2_item.Id = Guid.NewGuid();
                                            rb2_item.key = rb2_item.Id.ToString();
                                            rb2_item.APIID = applyAPIList.Id;
                                            rb2_item.DataType = "RequestBody";
                                            rb2_item.ParentID = requestBody.Id;
                                            rb2_item.FileName = item_lv.Key;
                                            rb2_item.FileType = item_lv.Value.Type.ToString() == "None" ? "object" : item_lv.Value.Type.ToString();
                                            rb2_item.IsRequired = item_lv.Value.IsRequired;
                                            rb2_item.Description = item_lv.Value.Description;
                                            rb2_item.MaxLength = 0;
                                            rb2_item.DefaultValue = "";
                                            rb2_item.ReceivedValue = "";
                                            rb2_item.Path = "";

                                            if (rb2_item.FileType.ToLower() == "Array".ToLower())
                                            {
                                                var type_rB_item = item_lv.Value.Item?.ActualSchema?.Type.ToString() == "None"
                                                                     ? "arrayOfString"
                                                                     : "arrayOf" + item_lv.Value.Item?.ActualSchema?.Type.ToString();
                                                rb2_item.FileType = type_rB_item;
                                            }
                                            List<ApplyBodyDataListDto> applies = new List<ApplyBodyDataListDto>();
                                            findChilderLeve(item_lv, applies, applyAPIList, rb2_item, 0);
                                            if (applies.Count > 0)
                                            {
                                                rb2_item.Children = applies;
                                            }

                                            //过滤掉IsNullableRaw==null的数据
                                            if (item_lv.Value.IsNullableRaw != null)
                                            {
                                                if (requestBody.Children == null)
                                                {
                                                    requestBody.Children = new List<ApplyBodyDataListDto>();
                                                }
                                                requestBody.Children.Add(rb2_item);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        requestBody.FileType = "string";
                                    }
                                    applyAPIList.requestBodyList.Add(requestBody);
                                }
                            }
                            else
                            {
                                ApplyBodyDataListDto requestBody = new ApplyBodyDataListDto();
                                requestBody.DataSource = "数据导入";
                                requestBody.Id = Guid.NewGuid();
                                requestBody.APIID = applyAPIList.Id;
                                requestBody.key = requestBody.Id.ToString();
                                requestBody.DataType = "RequestBody";
                                requestBody.ParentID = Guid.Empty;
                                requestBody.FileName = "RequestBody";
                                requestBody.FileType = "string";
                                requestBody.IsRequired = true;
                                requestBody.Description = "";
                                requestBody.MaxLength = 0;
                                requestBody.DefaultValue = "";
                                requestBody.ReceivedValue = "";
                                requestBody.Path = "RequestBody";

                                applyAPIList.requestBodyList.Add(requestBody);
                            }

                            ///返回body参数
                            if (operation.Value.Responses != null)
                            {
                                //reponse 返回参数
                                var responseContent = operation.Value.Responses.FirstOrDefault();
                                if (responseContent.Key != null && responseContent.Value != null)
                                {
                                    //reponse 返回参数 类型
                                    ApplyBodyDataListDto responseBody = new ApplyBodyDataListDto();
                                    responseBody.DataSource = "数据导入";
                                    responseBody.Id = Guid.NewGuid();
                                    responseBody.key = responseBody.Id.ToString();
                                    responseBody.APIID = applyAPIList.Id;
                                    responseBody.DataType = "ResponseBody";
                                    responseBody.ParentID = Guid.Empty;
                                    responseBody.FileName = "ResponseBody";
                                    responseBody.FileType = "string";
                                    responseBody.IsRequired = true;
                                    responseBody.Description = "";
                                    responseBody.MaxLength = 0;
                                    responseBody.DefaultValue = "";
                                    responseBody.ReceivedValue = "";
                                    responseBody.Path = "ResponseBody";

                                    var reponseType = responseContent.Value?.Schema?.Type.ToString();
                                    reponseType = reponseType == null ? "null" : reponseType;
                                    if (reponseType.ToLower() == "None".ToLower())
                                    {
                                        responseBody.FileType = "object";
                                        //reponse 返回参数 映射的实体
                                        if (responseContent.Value.Schema.Reference != null)
                                        {
                                            foreach (var resitem in responseContent.Value.Schema.Reference.ActualProperties)
                                            {
                                                ApplyBodyDataListDto rB_item = new ApplyBodyDataListDto();
                                                rB_item.DataSource = "数据导入";
                                                rB_item.Id = Guid.NewGuid();
                                                rB_item.key = rB_item.Id.ToString();
                                                rB_item.APIID = applyAPIList.Id;
                                                rB_item.DataType = "ResponseBody";
                                                rB_item.ParentID = responseBody.Id;
                                                rB_item.FileName = resitem.Key;
                                                rB_item.FileType = resitem.Value.Type.ToString() == "None" ? "object" : resitem.Value.Type.ToString();
                                                rB_item.IsRequired = resitem.Value.IsRequired;
                                                rB_item.Description = resitem.Value.Description;
                                                rB_item.MaxLength = 0;
                                                rB_item.DefaultValue = "";
                                                rB_item.ReceivedValue = "";
                                                rB_item.Path = "";

                                                if (rB_item.FileType.ToLower() == "Array".ToLower())
                                                {
                                                    var type_rB_item = resitem.Value.Item?.ActualSchema?.Type.ToString() == "None"
                                                                     ? "arrayOfString"
                                                                     : "arrayOf" + resitem.Value.Item?.ActualSchema?.Type.ToString();
                                                    rB_item.FileType = type_rB_item;
                                                }

                                                List<ApplyBodyDataListDto> applies = new List<ApplyBodyDataListDto>();
                                                findChilderLeve(resitem, applies, applyAPIList, rB_item, 0);
                                                if (applies.Count > 0)
                                                {
                                                    rB_item.Children = applies;
                                                }

                                                //过滤掉IsNullableRaw==null的数据
                                                if (resitem.Value.IsNullableRaw != null)
                                                {
                                                    if (responseBody.Children == null)
                                                    {
                                                        responseBody.Children = new List<ApplyBodyDataListDto>();
                                                    }
                                                    responseBody.Children.Add(rB_item);
                                                }
                                            }
                                        }
                                    }
                                    else if (reponseType.ToLower() == "array".ToLower())
                                    {
                                        var type = responseContent.Value.Schema?.Item?.ActualSchema?.Type.ToString() == "None"
                                                        ? "arrayOfString"
                                                        : "arrayOf" + responseContent.Value.Schema?.Item?.ActualSchema?.Type.ToString();
                                        responseBody.FileType = type;

                                        foreach (var item_lv in responseContent.Value.Schema.Item.ActualSchema.ActualProperties)
                                        {
                                            ApplyBodyDataListDto rb2_item = new ApplyBodyDataListDto();
                                            rb2_item.DataSource = "数据导入";
                                            rb2_item.Id = Guid.NewGuid();
                                            rb2_item.key = rb2_item.Id.ToString();
                                            rb2_item.APIID = applyAPIList.Id;
                                            rb2_item.DataType = "ResponseBody";
                                            rb2_item.ParentID = responseBody.Id;
                                            rb2_item.FileName = item_lv.Key;
                                            rb2_item.FileType = item_lv.Value.Type.ToString() == "None" ? "object" : item_lv.Value.Type.ToString();
                                            rb2_item.IsRequired = item_lv.Value.IsRequired;
                                            rb2_item.Description = item_lv.Value.Description;
                                            rb2_item.MaxLength = 0;
                                            rb2_item.DefaultValue = "";
                                            rb2_item.ReceivedValue = "";
                                            rb2_item.Path = "";

                                            if (rb2_item.FileType.ToLower() == "Array".ToLower())
                                            {
                                                var type_rB_item = item_lv.Value.Item?.ActualSchema?.Type.ToString() == "None"
                                                                 ? "arrayOfString"
                                                                 : "arrayOf" + item_lv.Value.Item?.ActualSchema?.Type.ToString();
                                                rb2_item.FileType = type_rB_item;
                                            }
                                            List<ApplyBodyDataListDto> applies = new List<ApplyBodyDataListDto>();
                                            findChilderLeve(item_lv, applies, applyAPIList, rb2_item, 0);
                                            if (applies.Count > 0)
                                            {
                                                rb2_item.Children = applies;
                                            }

                                            //过滤掉IsNullableRaw==null的数据
                                            if (item_lv.Value.IsNullableRaw != null)
                                            {
                                                if (responseBody.Children == null)
                                                {
                                                    responseBody.Children = new List<ApplyBodyDataListDto>();
                                                }
                                                responseBody.Children.Add(rb2_item);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //默认string类型
                                        responseBody.FileType = "string";
                                    }
                                    applyAPIList.responseBodyList.Add(responseBody);
                                }
                            }
                            else
                            {
                                ///没有response body给一个默认行
                                ApplyBodyDataListDto responseBody = new ApplyBodyDataListDto();
                                responseBody.DataSource = "数据导入";
                                responseBody.Id = Guid.NewGuid();
                                responseBody.key = responseBody.Id.ToString();
                                responseBody.APIID = applyAPIList.Id;
                                responseBody.DataType = "ResponseBody";
                                responseBody.ParentID = Guid.Empty;
                                responseBody.FileName = "ResponseBody";
                                responseBody.FileType = "string";
                                responseBody.IsRequired = true;
                                responseBody.Description = "";
                                responseBody.MaxLength = 0;
                                responseBody.DefaultValue = "";
                                responseBody.ReceivedValue = "";
                                responseBody.Path = "ResponseBody";
                                applyAPIList.responseBodyList.Add(responseBody);
                            }
                        }

                        applyAPIList.qhDataListDtos = qHDataListDtos;
                        improtApplyAPI.Add(applyAPIList);
                    }
                }
                //重复数据有多少
                if (improtApplyAPI.Where(t => t.IsRepeat).Count() > 0)
                {
                    improtApplyAPI.FirstOrDefault().CuntsRepeat = improtApplyAPI.Where(t => t.IsRepeat).Count();
                }
                return improtApplyAPI;
            }
            catch (Exception)
            {
                return improtApplyAPI;
            }
        }

        /// <summary>
        /// 确认导入
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SureImprotSwagger(List<ImprotSwaggerData> swagger, Guid applyID)
        {
            List<ApplyQHDataListDto> qhdata = new List<ApplyQHDataListDto>();
            List<ApplyBodyDataListDto> bddata = new List<ApplyBodyDataListDto>();

            //已有数据
            var OldappluApiList = await _applyAPIManager.QueryAsNoTracking
                                  .Where(t => t.ApplyID == applyID)
                                  .ToListAsync();

            List<Guid> del_qh = new List<Guid>();
            List<Guid> del_bd = new List<Guid>();

            //重复自动更新
            foreach (var item in swagger)
            {
                //处理更新
                if (item.IsRepeat)
                {
                    var oldapplyapi = OldappluApiList.Where(t => t.Path == item.Path).FirstOrDefault();
                    foreach (var qh_item in item.qhDataListDtos)
                    {
                        qh_item.APIID = oldapplyapi.Id;
                    }
                    qhdata.AddRange(item.qhDataListDtos);
                    del_qh.Add(oldapplyapi.Id);

                    List<ApplyBodyDataListDto> request_body = new List<ApplyBodyDataListDto>();
                    findApplyBodyData(item.requestBodyList, request_body);
                    foreach (var req_item in request_body)
                    {
                        req_item.APIID = oldapplyapi.Id;
                    }
                    bddata.AddRange(request_body);

                    List<ApplyBodyDataListDto> response_body = new List<ApplyBodyDataListDto>();
                    findApplyBodyData(item.responseBodyList, response_body);
                    foreach (var res_item in response_body)
                    {
                        res_item.APIID = oldapplyapi.Id;
                    }
                    bddata.AddRange(response_body);
                    del_bd.Add(oldapplyapi.Id);
                }
                else
                {
                    qhdata.AddRange(item.qhDataListDtos);

                    List<ApplyBodyDataListDto> request_body = new List<ApplyBodyDataListDto>();
                    findApplyBodyData(item.requestBodyList, request_body);
                    bddata.AddRange(request_body);

                    List<ApplyBodyDataListDto> response_body = new List<ApplyBodyDataListDto>();
                    findApplyBodyData(item.responseBodyList, response_body);
                    bddata.AddRange(response_body);
                }
            }

            var applyapi_entity = ObjectMapper.Map<List<ApplyAPI>>(swagger.Where(t => !t.IsRepeat));

            var applyapi_QH_entity = ObjectMapper.Map<List<ApplyQHData>>(qhdata);

            var applyapi_BD_entity = ObjectMapper.Map<List<ApplyBodyData>>(bddata);

            del_qh = _qhDataManager.QueryAsNoTracking.Where(t => del_qh.Contains(t.APIID)).Select(t => t.Id).ToList();
            del_bd = _bodyDataManager.QueryAsNoTracking.Where(t => del_bd.Contains(t.APIID)).Select(t => t.Id).ToList();
            ///事务提交
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                await _applyAPIManager.BatchCreateAsync(applyapi_entity);

                await _qhDataManager.BatchDelete(del_qh);
                await _qhDataManager.CreateAsync(applyapi_QH_entity);

                await _bodyDataManager.BatchDelete(del_bd);
                await _bodyDataManager.CreateAsync(applyapi_BD_entity);

                unitOfWork.Complete();
            }

            return true;
        }

        /// <summary>
        /// 找二级层级及其一下（response request）
        /// </summary>
        /// <param name="ActualProperties"></param>
        /// <param name="listDtos"></param>
        private void findChilderLeve(KeyValuePair<string, JsonProperty> ActualProperties,
            List<ApplyBodyDataListDto> listDtos,
            ImprotSwaggerData applyAPIList,
            ApplyBodyDataListDto parent,
            int leve,
            KeyValuePair<string, JsonProperty>? parentObject = null)
        {
            leve++;
            if (leve >= 5)
            {
                return;
            }

            if (ActualProperties.Value.Type.ToString().ToLower() == "object".ToLower())
            {
                if (ActualProperties.Value.Item == null)
                {
                    return;
                }
                foreach (var item_lv in ActualProperties.Value.Item.ActualSchema.ActualProperties)
                {
                    ApplyBodyDataListDto rb2_item = new ApplyBodyDataListDto();
                    rb2_item.DataSource = "数据导入";
                    rb2_item.Id = Guid.NewGuid();
                    rb2_item.key = rb2_item.Id.ToString();
                    rb2_item.APIID = applyAPIList.ApplyID;
                    rb2_item.DataType = parent.DataType;
                    rb2_item.ParentID = parent.Id;
                    rb2_item.FileName = item_lv.Key;
                    rb2_item.FileType = item_lv.Value.Type.ToString() == "None" ? "object" : item_lv.Value.Type.ToString();
                    rb2_item.IsRequired = item_lv.Value.IsRequired;
                    rb2_item.Description = item_lv.Value.Description;
                    rb2_item.MaxLength = 0;
                    rb2_item.DefaultValue = "";
                    rb2_item.ReceivedValue = "";
                    rb2_item.Path = "";
                    List<ApplyBodyDataListDto> applies = new List<ApplyBodyDataListDto>();
                    findChilderLeve(item_lv, applies, applyAPIList, rb2_item, leve, ActualProperties);
                    if (applies.Count > 0)
                    {
                        rb2_item.Children = applies;
                    }
                    if (listDtos == null)
                    {
                        listDtos = new List<ApplyBodyDataListDto>();
                    }
                    listDtos.Add(rb2_item);
                }
            }
            else if (ActualProperties.Value.Type.ToString().ToLower() == "None".ToLower())
            {
                if (ActualProperties.Value.ActualSchema == null)
                {
                    return;
                }
                foreach (var item_lv in ActualProperties.Value.ActualSchema.ActualProperties)
                {
                    ApplyBodyDataListDto rb2_item = new ApplyBodyDataListDto();
                    rb2_item.DataSource = "数据导入";
                    rb2_item.Id = Guid.NewGuid();
                    rb2_item.key = rb2_item.Id.ToString();
                    rb2_item.APIID = applyAPIList.ApplyID;
                    rb2_item.DataType = parent.DataType;
                    rb2_item.ParentID = parent.Id;
                    rb2_item.FileName = item_lv.Key;
                    rb2_item.FileType = item_lv.Value.Type.ToString() == "None" ? "object" : item_lv.Value.Type.ToString();
                    rb2_item.IsRequired = item_lv.Value.IsRequired;
                    rb2_item.Description = item_lv.Value.Description;
                    rb2_item.MaxLength = 0;
                    rb2_item.DefaultValue = "";
                    rb2_item.ReceivedValue = "";
                    rb2_item.Path = "";
                    List<ApplyBodyDataListDto> applies = new List<ApplyBodyDataListDto>();
                    findChilderLeve(item_lv, applies, applyAPIList, rb2_item, leve, ActualProperties);
                    if (applies.Count > 0)
                    {
                        rb2_item.Children = applies;
                    }
                    if (listDtos == null)
                    {
                        listDtos = new List<ApplyBodyDataListDto>();
                    }
                    listDtos.Add(rb2_item);
                }
            }
            else if (ActualProperties.Value.Type.ToString().ToLower() == "Array".ToLower())
            {
                ///预处理  子级是否是递归类型
                if (ActualProperties.Value.ParentSchema != null &&
                    ActualProperties.Value.ParentSchema?.ActualProperties?.Count == ActualProperties.Value.Item?.ActualSchema?.ActualProperties?.Count)
                {
                    bool isLike = true;
                    foreach (var item in ActualProperties.Value.ParentSchema.ActualProperties)
                    {
                        if (ActualProperties.Value.Item?.ActualSchema?.ActualProperties.ContainsKey(item.Key.ToString()) == false)
                        {
                            isLike = false;
                            break;
                        }
                    }
                    if (isLike)
                    {
                        return;
                    };
                }

                foreach (var item_lv in ActualProperties.Value.Item?.ActualSchema?.ActualProperties)
                {
                    ApplyBodyDataListDto rb2_item = new ApplyBodyDataListDto();
                    rb2_item.DataSource = "数据导入";
                    rb2_item.Id = Guid.NewGuid();
                    rb2_item.key = rb2_item.Id.ToString();
                    rb2_item.APIID = applyAPIList.ApplyID;
                    rb2_item.DataType = parent.DataType;
                    rb2_item.ParentID = parent.Id;
                    rb2_item.FileName = item_lv.Key;
                    rb2_item.FileType = item_lv.Value.Type.ToString() == "None" ? "object" : item_lv.Value.Type.ToString();
                    rb2_item.IsRequired = item_lv.Value.IsRequired;
                    rb2_item.Description = item_lv.Value.Description;
                    rb2_item.MaxLength = 0;
                    rb2_item.DefaultValue = "";
                    rb2_item.ReceivedValue = "";
                    rb2_item.Path = "";

                    if (rb2_item.FileType.ToLower() == "Array".ToLower())
                    {
                        var type_rB_item = item_lv.Value.Item?.ActualSchema?.Type.ToString() == "None"
                                             ? "arrayOfString"
                                             : "arrayOf" + item_lv.Value.Item?.ActualSchema?.Type.ToString();
                        rb2_item.FileType = type_rB_item;
                    }
                    List<ApplyBodyDataListDto> applies = new List<ApplyBodyDataListDto>();
                    findChilderLeve(item_lv, applies, applyAPIList, rb2_item, leve, ActualProperties);
                    if (applies.Count > 0)
                    {
                        rb2_item.Children = applies;
                    }
                    if (listDtos == null)
                    {
                        listDtos = new List<ApplyBodyDataListDto>();
                    }
                    listDtos.Add(rb2_item);
                }
            }

            return;
        }

        // 递归遍历 JToken，并移除循环引用
        private void RemoveCircularReferences(JToken jToken)
        {
            if (jToken == null || jToken.Type != JTokenType.Object)
            {
                return;
            }

            foreach (var child in jToken.Children<JProperty>().ToList())
            {
                if (child.Value is JValue || child.Value["properties"] == null)
                {
                    continue;
                }

                var properties = child.Value["properties"];
                foreach (var child2 in properties.Children<JProperty>().ToList())
                {
                    if (child2.Value["$ref"] != null)
                    {
                        var refs = child2.Value["$ref"].ToString();
                        refs = refs.Replace("#/", "");
                        refs = refs.Replace("/", ".");

                        if (child2.Path.Length > refs.Length)
                        {
                            var parentpath = child2.Path.Substring(0, refs.Length);

                            if (parentpath == refs)
                            {
                                child2.Value["$ref"].Parent.Remove();
                            }
                        }
                    }
                }
            }
        }

        // 找到所有子级放到list
        private List<ApplyBodyDataListDto> findApplyBodyData(List<ApplyBodyDataListDto> applyQHDatas, List<ApplyBodyDataListDto> qHDatas)
        {
            foreach (var item in applyQHDatas)
            {
                qHDatas.Add(item);
                if (item.Children != null && item.Children.Count > 0)
                {
                    findApplyBodyData(item.Children, qHDatas);
                }
            }
            return qHDatas;
        }
    }
}
