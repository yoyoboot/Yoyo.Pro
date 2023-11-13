// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Data;
using System.Threading.Tasks;
using Abp.Dependency;
using SqlSugar;

namespace Yoyo.LowCode.LowCodeAPI.DomainService.SqlService
{
    public interface ISqlHelperManager : ITransientDependency
    {
        /// <summary>
        /// 列表查询
        /// </summary>
        /// <param name="connectionStr"></param>
        /// <param name="sqlStr"></param>
        /// <param name="maxResultCount"></param>
        /// <param name="skipCount"></param>
        /// <param name="isPaged"></param>
        /// <returns></returns>
        Task<DataTable> GetPaged(string dbType, string connectionStr, string sqlStr, SugarParameter[] param = null);

        public string ToConnectionString(string dbType, string Host, int? Port, string UserName, string Password, string ServiceName);
    }
}
