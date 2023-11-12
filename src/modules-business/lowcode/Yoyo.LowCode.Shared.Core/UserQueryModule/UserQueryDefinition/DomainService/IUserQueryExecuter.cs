// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Yoyo.LowCode.UserQueryModule.UserQueryDefinition.DomainService
{
    public interface IUserQueryExecuter
    {
        /// <summary>
        /// 执行查询-从 <see cref="BaseUserQuery.QueryParamters"/> 获取参数
        /// </summary>
        /// <param name="query">查询配置</param>
        /// <returns></returns>
        Task<DataTable> Execute(BaseUserQuery query);

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="query">查询配置</param>
        /// <param name="parms">查询参数</param>
        /// <returns></returns>
        Task<DataTable> Execute(BaseUserQuery query, Dictionary<string, object> parms);

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="queryTemplate"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        Task<DataTable> Execute(string queryTemplate, Dictionary<string, object> parms);
    }
}
