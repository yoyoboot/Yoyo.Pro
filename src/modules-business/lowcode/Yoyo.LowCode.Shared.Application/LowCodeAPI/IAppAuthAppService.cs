// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yoyo.LowCode.LowCodeAPI.Dtos.AppAuthDto;

namespace Yoyo.LowCode.LowCodeAPI
{
    public interface IAppAuthAppService : IApplicationService
    {
        /// <summary>
        /// 获取授权应用列表
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<AppAuthListDto>> GetAppAuthList(GetAppAuthPagesInput input);

        /// <summary>
        /// 通过指定id获取 获取接口中心 授权应用
        /// </summary>
        Task<AppAuthListDto> GetById(EntityDto<Guid> input);

        /// <summary>
        /// 添加或者修改 API接口数据 的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdateAppAuth input);

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
        /// 获取接口中心应用下  AppAuthMapping 列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<AppAuthMappingListDto>> GetAppAuthMappingList(GetAppAuthMappingPagesInput input);

        /// <summary>
        /// 新增修改 AppAuthMapping
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateForAppAuthMapping(CreateOrUpdateAppAuthMapping input);

        /// <summary>
        /// 校验 key密钥是否拥有这个api授权(0 校验成功，1 key值有误，2 应用下没有这个授权)
        /// </summary>
        /// <param name="AppKey"></param>
        /// <param name="AgentID"></param>
        /// <returns></returns>
        Task<int> CheckAppAuthYoyoApiGatewayToken(string AppKey, Guid AgentID);
    }
}
