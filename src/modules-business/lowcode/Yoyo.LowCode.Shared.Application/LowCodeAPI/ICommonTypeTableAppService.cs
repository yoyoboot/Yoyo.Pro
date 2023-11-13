// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yoyo.LowCode.LowCodeAPI.Dtos.CommonTypeTableDto;

namespace Yoyo.LowCode.LowCodeAPI
{
    public interface ICommonTypeTableAppService : IApplicationService
    {
        /// <summary>
        /// 获取系统参数配置信息（TableName）
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<CommonTypeTableListDto>> GetApplyAPIList(GetCommonTypeTablePagesInput input);

        /// <summary>
        /// 通过指定id获取 系统参数配置信息
        /// </summary>
        Task<CommonTypeTableListDto> GetById(EntityDto<Guid> input);

        /// <summary>
        /// 添加或者修改 系统参数配置信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdateCommonTypeTable input);

        /// <summary>
        /// 删除 系统参数配置信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<Guid> input);

        /// <summary>
        /// 批量删除 系统参数配置信息
        /// </summary>
        Task BatchDelete(List<Guid> input);
    }
}
