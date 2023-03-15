// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Services;
using System.Data;
using System.Linq;
using System.Text;

namespace Yoyo.Pro.Database
{
    /// <summary>
    /// SQL 执行器，用于执行一些 SQL 语句。
    /// </summary>
    public interface ISqlExecutor : IDomainService
    {
        /// <summary>
        /// 执行一条 SQL 语句并将查询结果返回。
        /// </summary>
        /// <typeparam name="TAny">查询的实体类型。</typeparam>
        /// <param name="query">SQL 语句。</param>
        /// <param name="parameters">SQL 参数。</param>
        /// <returns>SQL 查询结果。</returns>
        IEnumerable<TAny> Query<TAny>(string query, object parameters = null)
            where TAny : class;

        /// <summary>
        /// 执行一条 SQL 语句并将查询结果返回。
        /// </summary>
        /// <typeparam name="TAny">查询的实体类型。</typeparam>
        /// <param name="query">SQL 语句。</param>
        /// <param name="parameters">SQL 参数。</param>
        /// <returns>SQL 查询结果。</returns>
        Task<IEnumerable<TAny>> QueryAsync<TAny>(string query, object parameters = null)
            where TAny : class;

        /// <summary>
        /// 执行一条 SQL 语句并将执行结果数量返回。
        /// </summary>
        /// <param name="query">SQL 语句。</param>
        /// <param name="parameters">SQL 参数。</param>
        /// <returns>执行结果数量。</returns>
        int Execute(string query, object parameters = null);

        /// <summary>
        /// 执行一条 SQL 语句并将执行结果数量返回。
        /// </summary>
        /// <param name="query">SQL 语句。</param>
        /// <param name="parameters">SQL 参数。</param>
        /// <returns>执行结果数量。</returns>
        Task<int> ExecuteAsync(string query, object parameters = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<IDataReader> ExecuteReaderAsync(string query, object parameters = null);

        /// <summary>
        /// 获取数据库链接
        /// </summary>
        /// <returns></returns>
        IDbConnection GetConnection();

        /// <summary>
        /// 获取数据库链接
        /// </summary>
        /// <returns></returns>
        Task<IDbConnection> GetConnectionAsync();

        /// <summary>
        /// 获取事务
        /// </summary>
        /// <returns></returns>
        IDbTransaction GetActiveTransaction();

        /// <summary>
        /// 获取事务
        /// </summary>
        /// <returns></returns>
        Task<IDbTransaction> GetActiveTransactionAsync();
    }
}
