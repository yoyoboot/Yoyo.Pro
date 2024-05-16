// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yoyo.LowCode.LowCodeAPI.Dtos.ApplyTypeDto;

namespace Yoyo.LowCode.LowCodeAPI
{
    public interface IApplyTypeAppService : IApplicationService
    {
        /// <summary>
        /// 应用分类类型 下拉列表
        /// </summary>
        /// <returns></returns>
        Task<List<ApplyTypeListDto>> SelectApplyType();

        /// <summary>
        /// 添加或者修改 应用分类 的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdateApplyType input);

        /// <summary>
        /// 删除 应用分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<Guid> input);

        /// <summary>
        /// 批量删除 应用分类
        /// </summary>
        Task BatchDelete(List<Guid> input);
    }
}
