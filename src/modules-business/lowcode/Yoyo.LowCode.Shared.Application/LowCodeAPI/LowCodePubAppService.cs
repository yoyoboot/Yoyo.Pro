// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using Yoyo.LowCode.LowCodeAPI.DomainService;
using Yoyo.LowCode.LowCodeAPI.Dtos.Modeling;

namespace Yoyo.LowCode.LowCodeAPI
{
    public class LowCodePubAppService : LowCodeSharedAppServiceBase, ILowCodePubAppService
    {
        private readonly IApplyAPIManager _applyAPIManager;

        private readonly IAgentAPIManager _agentAPIManager;
        private readonly IApplyQHDataManager _qhDataManager;
        private readonly IApplyBodyDataManager _bodyDataManager;

        public LowCodePubAppService(IAgentAPIManager agentAPIManager,
            IApplyQHDataManager qhDataManager,
            IApplyBodyDataManager bodyDataManager,
            IApplyAPIManager applyAPIManager)
        {
            _agentAPIManager = agentAPIManager;
            _qhDataManager = qhDataManager;
            _bodyDataManager = bodyDataManager;
            _applyAPIManager = applyAPIManager;
        }

        /// <summary>
        /// 获取API下拉列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<DataApiList>> GetAPIList(GetAPIPagesInput input)
        {
            var query = _agentAPIManager.Query
              .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a =>
                a.Name.Contains(input.FilterText) ||
                a.Path.Contains(input.FilterText)
            ).WhereIf(!input.LableName.IsNullOrWhiteSpace(), a =>
                a.LableName == a.LableName);

            var count = await query.CountAsync();

            var DataList = await query
                .OrderBy(input.Sorting).AsNoTracking()
                .PageBy(input)
                .ToListAsync();

            var DataListDtos = ObjectMapper.Map<List<DataApiList>>(DataList);

            return new PagedResultDto<DataApiList>(count, DataListDtos);
        }

        /// <summary>
        /// 获取API参数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DataApiParameter> GetAPIParameter(Guid id)
        {
            //返回的数据信息
            DataApiParameter parameter = new DataApiParameter();
            //当前api信息
            var agentApi = await _agentAPIManager.QueryAsNoTracking.Where(t => t.Id == id).FirstOrDefaultAsync();

            List<DataApiField> queryfileds = new List<DataApiField>();
            ////query入参
            var queryEntity = await _qhDataManager.QueryAsNoTracking.Where(t => t.APIID == id && t.DataType.ToLower() == "Query".ToLower()).ToListAsync();
            foreach (var item in queryEntity)
            {
                DataApiField queryitem = new DataApiField();
                queryitem.FieldName = item.FileName;
                queryitem.DisplayName = item.Description;
                queryitem.FieldType = item.FileType;
                queryitem.IsRequired = item.IsRequired;
                queryfileds.Add(queryitem);
            }
            parameter.Querys = queryfileds;

            List<DataApiField> headerfileds = new List<DataApiField>();
            ////header入参
            var headerEntity = await _qhDataManager.QueryAsNoTracking.Where(t => t.APIID == id && t.DataType.ToLower() == "header".ToLower()).ToListAsync();
            foreach (var item in headerEntity)
            {
                DataApiField headeritem = new DataApiField();
                headeritem.FieldName = item.FileName;
                headeritem.DisplayName = item.Description;
                headeritem.FieldType = item.FileType;
                headeritem.IsRequired = item.IsRequired;
                headerfileds.Add(headeritem);
            }
            parameter.Headers = headerfileds;

            //Body入参
            var requestBody = await _bodyDataManager.QueryAsNoTracking.Where(t => t.APIID == id && t.DataType.ToLower() == "RequestBody".ToLower()).ToListAsync();
            //组装数据
            List<DataApiField> requestBodyList = new List<DataApiField>();
            var requestDatas = requestBody.Where(t => t.ParentID == Guid.Empty).FirstOrDefault();
            if (requestDatas != null)
            {
                DataApiField requestItem = new DataApiField();
                requestItem.FieldName = requestDatas.FileName;
                requestItem.DisplayName = requestDatas.Description;
                requestItem.FieldType = requestDatas.FileType;
                requestItem.IsRequired = requestDatas.IsRequired;
                requestItem.Children = FindChildren(requestBody, null);
                requestBodyList.Add(requestItem);
            }
            parameter.RequestBody = requestBodyList;

            //Body 返回参数(目前都是透传 直接找到具体映射（http  sqlserver  mock）)
            if (agentApi.ServiceName.ToLower() == "http".ToLower())
            {
                var responseBody = await _bodyDataManager.QueryAsNoTracking.Where(t => t.APIID == agentApi.ApplyAPIID && t.DataType.ToLower() == "ResponseBody".ToLower()).ToListAsync();
                //组装数据
                List<DataApiField> responseBodyList = new List<DataApiField>();
                var reponseDatas = responseBody.Where(t => t.ParentID == Guid.Empty).FirstOrDefault();
                if (reponseDatas != null)
                {
                    DataApiField reponseItem = new DataApiField();
                    reponseItem.FieldName = reponseDatas.FileName;
                    reponseItem.DisplayName = reponseDatas.Description;
                    reponseItem.FieldType = reponseDatas.FileType;
                    reponseItem.IsRequired = reponseDatas.IsRequired;
                    reponseItem.Children = FindChildren(responseBody, null);
                    responseBodyList.Add(reponseItem);
                }
                parameter.ResponseBody = responseBodyList;
            }

            return parameter;
        }

        /// <summary>
        /// 组装数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="recordId"></param>
        /// <returns></returns>
        internal List<DataApiField> FindChildren(List<ApplyBodyData> data, Guid? recordId)
        {
            if (data.Count <= 0)
            {
                return null;
            }
            List<DataApiField> treeList = new List<DataApiField>();
            recordId = recordId == null ? data.Where(t => t.ParentID == Guid.Empty).FirstOrDefault().Id : recordId;
            //获取id的所有下级
            List<ApplyBodyData> dataList = data.Where(u => u.ParentID == recordId).ToList();
            foreach (var item in dataList)
            {
                DataApiField model = new DataApiField();
                model.FieldName = item.FileName;
                model.DisplayName = item.Description;
                model.FieldType = item.FileType;
                model.IsRequired = item.IsRequired;
                //循环调用，获取当前的子目录
                model.Children = FindChildren(data, item.Id);
                treeList.Add(model);
            }
            return treeList.Count() > 0 ? treeList : null;
        }
    }
}
