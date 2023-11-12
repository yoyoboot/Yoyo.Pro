// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yoyo.LowCode.LowCodeAPI.Dtos.ApplyHealthTestingDto;

namespace Yoyo.LowCode.LowCodeAPI
{
    public interface IApplyHealthTestingAppService : IApplicationService
    {
        /// <summary>
        /// 通过指定id获取 获取API网关应用 下的接口健康检测规则 数据
        /// </summary>
        Task<ApplyHealthTestingListDto> GetById(EntityDto<Guid> input);

        /// <summary>
        /// 通过指定ApplyID获取 获取API网关应用 下的接口健康检测规则 数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ApplyHealthTestingListDto> GetByApplyID(EntityDto<Guid> input);

        /// <summary>
        /// 添加或者修改 接口健康检测规则 的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdateApplyHealthTesting input);

        /// <summary>
        /// 删除 接口健康检测规则
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<Guid> input);

        /// <summary>
        /// 批量删除 接口健康检测规则
        /// </summary>
        Task BatchDelete(List<Guid> input);
    }
}
