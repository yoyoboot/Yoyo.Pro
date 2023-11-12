// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Yoyo.Pro.Domain;

namespace Yoyo.LowCode.UserQueryModule.UserQueryRelationalDefinition.DomainService
{
    /// <summary>
    /// User Query Group 和 User Query 中间表
    /// </summary>
    public interface IUserQueryGroupEntityManager : IBasicDomainService<BaseUserQueryGroupEntity, Guid>
    {
        /// <summary>
        /// 根据 BaseUserQuery id 判断是否已经被使用
        /// </summary>
        /// <param name="userQueryId"></param>
        /// <returns></returns>
        Task<bool> UserQueryHasBeenUsed(Guid userQueryId);

        /// <summary>
        /// 根据组来删除 与之关联的Item
        /// </summary>
        /// <param name="inputId"></param>
        /// <returns></returns>
        Task DeleteGroupItemByGroupId(Guid inputId);
    }
}
