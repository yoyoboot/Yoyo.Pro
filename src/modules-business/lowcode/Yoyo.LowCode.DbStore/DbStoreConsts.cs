// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Yoyo.LowCode
{
    public static class DbStoreConsts
    {
        /// <summary>
        /// Guid最大长度
        /// </summary>
        public const int GuidMaxCount = 20;

        /// <summary>
        /// 备注 最大长度
        /// </summary>
        public const int RemarkMaxCount = 2000;

        public const string Base = "Base";

        public const string Sort = "CreationTime desc";

        public const string LocalizationSourceName = "YoyoLowCode";

        public const string SqlServer = "sqlserver";

        public const string DataSqlClient = "system.data.sqlclient";

        public const string SqlClientSqlConnection = "system.data.sqlclient.sqlconnection";

        public const string MicrosoftSqlClientSqlConnection = "microsoft.data.sqlclient.sqlconnection";

        public const string Oracle = "oracle";

        public const string MySql = "mysql";

        public const string MySqlClientMySqlConnection = "mysql.data.mysqlclient.mysqlconnection";
    }
}
