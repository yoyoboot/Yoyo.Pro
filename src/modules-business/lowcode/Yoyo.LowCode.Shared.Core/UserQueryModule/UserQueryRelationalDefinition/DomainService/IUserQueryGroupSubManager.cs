// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Yoyo.Pro.Domain;

namespace Yoyo.LowCode.UserQueryModule.UserQueryRelationalDefinition.DomainService
{
    /// <summary>
    /// 查询组关联查询组领域服务
    /// </summary>
    public interface IUserQueryGroupSubManager : IBasicDomainService<BaseQueryGroupSub, Guid>
    {
        /// <summary>
        /// 根据 BaseUserQueryGroup id 判断是否已经被使用
        /// </summary>
        /// <param name="userQueryGroupId"></param>
        /// <returns></returns>
        Task<bool> UserQueryGroupHasBeenUsed(Guid userQueryGroupId);

        /// <summary>
        /// 根据父组 删除子组
        /// </summary>
        /// <param name="inputId"></param>
        /// <returns></returns>
        Task DeleteSubGroupByGroupId(Guid inputId);
    }
}
