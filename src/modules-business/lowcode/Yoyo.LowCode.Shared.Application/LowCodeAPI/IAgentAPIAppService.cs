// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yoyo.LowCode.LowCodeAPI.Dtos.AgentAPIDto;
using Yoyo.LowCode.LowCodeAPI.Dtos.APIRequestRecordDto;

namespace Yoyo.LowCode.LowCodeAPI
{
    /// <summary>
    /// 接口中心API
    /// </summary>
    public interface IAgentAPIAppService : IApplicationService
    {
        /// <summary>
        /// 获取接口中心 接口列表
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<AgentAPIListDto>> GetAgentAPIList(GetAgentAPIPagesInput input);

        /// <summary>
        /// 通过指定id  获取获取接口中心 API接口数据
        /// </summary>
        Task<AgentAPIListDto> GetById(EntityDto<Guid> input);

        /// <summary>
        /// 添加或者修改 接口中心API接口数据 的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdateAgentAPI input);

        /// <summary>
        /// 删除 接口中心API接口数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<Guid> input);

        /// <summary>
        /// 批量删除 接口中心API接口数据
        /// </summary>
        Task BatchDelete(List<Guid> input);

        /// <summary>
        /// 根据 请求路径 查询API信息
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        Task<AgentAPIListDto> GetAgentAPIByPath(string path);

        /// <summary>
        ///  返回所有接口信息(不分页)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<AgentAPIListAllDto>> GetAgentAPIListAll(GetAgentAPIPagesInput input);

        /// <summary>
        ///  添加或者修改 接口中心API接口数据(接口中心API 请求query header body 后端映射关系)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateAgentAPI(CreateOrUpdateAgentAPI input);

        /// <summary>
        /// 发布和下线
        /// </summary>
        /// <param name="input"></param>
        /// <param name="EnableStatus"></param>
        /// <returns></returns>
        Task SetAPIEnableStatus(EntityDto<Guid> input, bool enableStatus);

        /// <summary>
        /// 查询接口请求记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        Task<PagedResultDto<APIRequestRecordListDto>> GetAgentAPIRequestRecord(GetAPIRequestRecordPagesInput input);

        /// <summary>
        /// 查询 网关请求记录列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<APIRequestRecordListDto>> GetAgentAPIRequestRecordList(GetAPIRequestRecordPagesInput input);

        /// <summary>
        /// 查询基础统计信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<Dashboard_TJDto> GetDashboardTJ(Dashboard_TJInput input);
    }
}
