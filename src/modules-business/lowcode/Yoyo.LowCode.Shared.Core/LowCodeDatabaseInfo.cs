// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Configuration;

namespace Yoyo.LowCode
{
    public class LowCodeDatabaseInfo
    {
        public DatabaseTypeEnum DatabaseType { get; protected set; }

        protected LowCodeDatabaseInfo()
        {
            DatabaseType = DatabaseTypeEnum.SqlServer;
        }

        protected void LoadFromConfiguraion(IConfiguration configuration)
        {
            var databaseType = configuration.GetValue<string>("ConnectionStrings:DatabaseType")
                ?.ToLower()
                ?.Trim();

            switch (databaseType)
            {
                case "oracle":
                    DatabaseType = DatabaseTypeEnum.Oracle;
                    break;

                case "mysql":
                    DatabaseType = DatabaseTypeEnum.MySql;
                    break;

                case "sqlserver":
                default:
                    DatabaseType = DatabaseTypeEnum.SqlServer;
                    break;
            }
        }

        public static LowCodeDatabaseInfo Instance { get; } = new LowCodeDatabaseInfo();

        /// <summary>
        /// 加载数据库配置信息
        /// </summary>
        /// <param name="configuration"></param>
        public static void LoadConfiguraion(IConfiguration configuration)
        {
            LowCodeDatabaseInfo.Instance.LoadFromConfiguraion(configuration);
        }
    }

    public enum DatabaseTypeEnum
    {
        SqlServer = 0,
        Oracle = 1,
        MySql = 2,
    }
}
