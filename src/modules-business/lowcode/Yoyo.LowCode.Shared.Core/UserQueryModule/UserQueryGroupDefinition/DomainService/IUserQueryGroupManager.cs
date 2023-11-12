// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Yoyo.Pro.Domain;

namespace Yoyo.LowCode.UserQueryModule.UserQueryGroupDefinition.DomainService
{
    /// <summary>
    /// BaseUserQuery 的领域服务
    /// </summary>
    public interface IUserQueryGroupManager : IBasicDomainService<BaseUserQueryGroup, Guid>
    {
        /// <summary>
        /// 删除组 以及 当前组的联系表
        /// </summary>
        /// <param name="inputId"></param>
        /// <returns></returns>
        Task DeleteNdoAndEntry(Guid inputId);
    }
}
