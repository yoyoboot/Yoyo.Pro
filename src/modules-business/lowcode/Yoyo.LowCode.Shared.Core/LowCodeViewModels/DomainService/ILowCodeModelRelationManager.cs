// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yoyo.LowCode.LowCodeViewModels.Dtos;
using Yoyo.Pro.Domain;

namespace Yoyo.LowCode.LowCodeViewModels.DomainService
{
    public interface ILowCodeModelRelationManager : IBasicDomainService<LowCodeModelRelation, Guid>
    {
        /// <summary>
        /// 判断是否存在引用表
        /// </summary>
        /// <param name="tabelName"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        RefTableFieldModel IsReferenceTable(string tabelName, string fieldName);

        /// <summary>
        /// 批量创建关联关系
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<List<LowCodeModelRelation>> BlukOperateRelationAsync(List<LowCodeModelRelation> item);

        /// <summary>
        /// 删除关联关系
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(Guid id);

        Task BulkDeleteAsync(string tableName);
    }
}
