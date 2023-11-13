// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Data;
using System.Threading.Tasks;
using SqlSugar;
using Yoyo.LowCode.Databases.DomainService;
using Yoyo.LowCode.Extension;
using DatabaseType = SqlSugar.DbType;

namespace Yoyo.LowCode.LowCodeAPI.DomainService.SqlService
{
    public class SqlHelperManager : ISqlHelperManager
    {
        private readonly IDatabaseManager _databaseManager;

        public SqlHelperManager(IDatabaseManager databaseManager)
        {
            _databaseManager = databaseManager;
        }

        public async Task<DataTable> GetPaged(string dbType, string connectionStr, string sqlStr, SugarParameter[] param = null)
        {
            var table = await _databaseManager.ExecuteDataTableAsync(dbType, connectionStr, sqlStr, param);

            return table;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="Host"></param>
        /// <param name="Port"></param>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <param name="ServiceName"></param>
        /// <returns></returns>
        public string ToConnectionString(string dbType, string Host, int? Port, string UserName, string Password, string ServiceName)
        {
            var ConnectionStr = "0";
            if (dbType.ToUpper() == "sqlserver".ToUpper())
            {
                ConnectionStr = DataHelper.ToConnectionString(DatabaseType.SqlServer, Host, Port, UserName, Password, ServiceName);
            }
            else if (dbType.ToUpper() == "mysql".ToUpper())
            {
                ConnectionStr = DataHelper.ToConnectionString(DatabaseType.MySql, Host, Port, UserName, Password, ServiceName);
            }
            else if (dbType.ToUpper() == "oracle".ToUpper())
            {
                ConnectionStr = DataHelper.ToConnectionString(DatabaseType.Oracle, Host, Port, UserName, Password, ServiceName);
            }
            return ConnectionStr;
        }
    }
}
