// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yoyo.LowCode.ComField.Dtos;

namespace Yoyo.LowCode.ComField
{
    public interface IComFieldAppService : IApplicationService
    {
        /// <summary>
        /// 获取常用字段列表集合
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ComFieldListDto>> GetPaged(ComFieldInput input);

        /// <summary>
        /// 返回实体常用字段的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ComFieldForEditOutput> GetForEdit(NullableIdDto<Guid> input);

        /// <summary>
        /// 添加或者修改常用字段的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(ComFieldForCreateOrUpdate input);

        /// <summary>
        /// 删除常用字段
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<Guid> input);

        /// <summary>
        /// 获取数据库下拉列表
        /// </summary>
        /// <returns></returns>
        Task<List<ComFieldSelectorOutput>> GetComFieldSelector();

        //// custom codes

        //// custom codes end
    }
}
