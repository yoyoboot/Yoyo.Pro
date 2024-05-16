// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yoyo.LowCode.LowCodeAPI.Dtos.FlowControlDto;

namespace Yoyo.LowCode.LowCodeAPI
{
    public interface IFlowControlAppService : IApplicationService
    {
        /// <summary>
        /// 获取流量控制策略列表
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<FlowControlListDto>> GetFlowControlList(GetFlowControlPagesInput input);

        /// <summary>
        /// 通过指定id获取 获取接口中心 流量控制策略
        /// </summary>
        Task<FlowControlListDto> GetById(EntityDto<Guid> input);

        /// <summary>
        /// 添加或者修改 API接口数据 的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdateFlowControl input);

        /// <summary>
        /// 删除 API接口数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<Guid> input);

        /// <summary>
        /// 批量删除 API接口数据
        /// </summary>
        Task BatchDelete(List<Guid> input);

        /// <summary>
        /// 获取流量控制策略下  FlowControlMapping 列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<FlowControlMappingEditDto>> GetFlowControlMappingList(GetFlowControlMappingPagesInput input);

        /// <summary>
        /// 新增修改 FlowControlMapping
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateForFlowControlMapping(CreateOrUpdateFlowControlMapping input);

        /// <summary>
        /// 查询 AgentAPI启用的流量策略
        /// </summary>
        /// <param name="AgentID"></param>
        /// <returns></returns>
        Task<FlowControl> GetFlowControlByAgentID(Guid AgentID);
    }
}
