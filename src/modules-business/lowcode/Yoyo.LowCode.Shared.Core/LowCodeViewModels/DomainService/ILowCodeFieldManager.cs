// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yoyo.Pro.Domain;

namespace Yoyo.LowCode.LowCodeViewModels.DomainService
{
    public interface ILowCodeFieldManager : IBasicDomainService<LowCodeField, Guid>
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="lowCodeField"></param>
        /// <returns></returns>
        Task<LowCodeField> CreateAsync(LowCodeField lowCodeField);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="lowCodeField"></param>
        /// <returns></returns>
        Task UpdateAsync(LowCodeField lowCodeField);

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="lowCodeField"></param>
        /// <returns></returns>
        Task<List<LowCodeField>> BlukOperateFieldAsync(List<LowCodeField> entityList);

        /// <summary>
        /// 通过表名删除字段信息
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        Task BulkDeleteAsync(string tableName);

        Task MaintainFields(LowCodeField lowCodeField);
    }
}
