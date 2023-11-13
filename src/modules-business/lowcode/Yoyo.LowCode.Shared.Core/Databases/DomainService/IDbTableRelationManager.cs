// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yoyo.LowCode.Databases.Entity;
using Yoyo.Pro.Domain;

namespace Yoyo.LowCode.Databases.DomainService
{
    public interface IDbTableRelationManager : IBasicDomainService<BaseDbTableRelation, Guid>
    {
        /// <summary>
        /// 创建关联关系
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<BaseDbTableRelation> CreateAsync(BaseDbTableRelation entity);

        /// <summary>
        /// 批量创建关联关系
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<List<BaseDbTableRelation>> BlukCreateAsync(List<BaseDbTableRelation> item);

        /// <summary>
        /// 删除关联关系
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// 修改关联关系
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task UpdateAsync(BaseDbTableRelation entity);
    }
}
