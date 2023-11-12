// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yoyo.LowCode.LowCodeDefaultFields.Dtos;

namespace Yoyo.LowCode.LowCodeDefaultFields
{
    public interface ILowCodeDefaultFieldAppService : IApplicationService
    {
        /// <summary>
        ///  默认字段列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<LowCodeDefaultFieldListDto>> GetPaged(LowCodeDefaultFieldInput input);

        /// <summary>
        /// 添加或修改默认字段
        /// </summary>
        /// <param name="defaultField"></param>
        /// <returns></returns>
        Task CreateOrUpdate(LowCodeDefaultFieldCreateOrUpdate defaultField);

        /// <summary>
        /// 删除默认字段
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<Guid> input);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task BatchDelete(List<Guid> input);

        /// <summary>
        /// 获取要编辑的默认字段信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<LowCodeDefaultFieldForEditOutput> GetForEdit(NullableIdDto<Guid> input);
    }
}
