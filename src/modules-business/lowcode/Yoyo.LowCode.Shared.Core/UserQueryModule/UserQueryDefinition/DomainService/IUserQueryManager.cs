// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Yoyo.Pro.Domain;

namespace Yoyo.LowCode.UserQueryModule.UserQueryDefinition.DomainService
{
    /// <summary>
    /// BaseUserQuery 的领域服务
    /// </summary>
    public interface IUserQueryManager : IBasicDomainService<BaseUserQuery, Guid>
    {
        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="query">查询配置</param>
        /// <param name="parms">查询参数</param>
        /// <returns></returns>
        Task<DataTable> Execute(BaseUserQuery query, Dictionary<string, object> parms);
    }
}
