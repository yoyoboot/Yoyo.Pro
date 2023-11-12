// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yoyo.LowCode.LowCodeAPI.Dtos.ApplyAuthenticationDto;

namespace Yoyo.LowCode.LowCodeAPI
{
    /// <summary>
    /// 连接应用API 授权信息
    /// </summary>
    public interface IApplyAuthenticationAppService : IApplicationService
    {
        /// <summary>
        /// 通过指定id获取 获取API网关应用 下的身份鉴权数据
        /// </summary>
        Task<ApplyAuthenticationListDto> GetById(EntityDto<Guid> input);

        /// <summary>
        /// 通过ApplyID获取身份鉴权数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ApplyAuthenticationListDto> GetByApplyID(EntityDto<Guid> input);

        /// <summary>
        /// 添加或者修改 身份鉴权数据 的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdateApplyAuthentication input);

        /// <summary>
        /// 删除 身份鉴权数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<Guid> input);

        /// <summary>
        /// 批量删除 身份鉴权数据
        /// </summary>
        Task BatchDelete(List<Guid> input);
    }
}
