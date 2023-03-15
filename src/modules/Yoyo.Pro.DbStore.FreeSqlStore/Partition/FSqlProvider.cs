using System;
using FreeSql.Internal;
using Yoyo.Pro.DbStore;
using Yoyo.Pro.DbStore.FreeSqlStore.Configuration;

namespace Yoyo.Pro.DbStore.FreeSqlStore.Partition
{
    public sealed class FSqlProvider : IFSqlProvider
    {

        private readonly IFSqlSetting _fSqlSetting;
        private readonly IFreeSql _fsql;


        public string ProviderName { get => _fSqlSetting.Name; }

        public IFreeSql FSql => _fsql;


        public FSqlProvider(IFSqlSetting fSqlSetting)
        {
            _fSqlSetting = fSqlSetting;
            _fsql = CreateFSql(_fSqlSetting);
        }


        /// <summary>
        /// 创建实例fSql实例
        /// </summary>
        /// <param name="fSqlSetting"></param>
        /// <returns></returns>
        private IFreeSql CreateFSql(IFSqlSetting fSqlSetting)
        {
            var dbType = GetDbType(fSqlSetting.DatabaseType);

            var freeSqlBuilder = new FreeSql.FreeSqlBuilder()
                .UseConnectionString(dbType, fSqlSetting.ConnectionString)
                .UseAutoSyncStructure(false) // 同步数据库表结构,不启用,使用EFCore code first
                .UseLazyLoading(false);         // 懒加载

            switch (dbType)
            {
                case FreeSql.DataType.Oracle:
                    freeSqlBuilder.UseNameConvert(NameConvertType.ToUpper);
                    break;
                case FreeSql.DataType.PostgreSQL:
                    freeSqlBuilder.UseNameConvert(NameConvertType.ToLower);
                    break;
            }

            // 使用sql生成命令日志
            if (fSqlSetting.UseSqlExecuteLog && fSqlSetting.SqlExecuting != null)
            {

                freeSqlBuilder = freeSqlBuilder.UseMonitorCommand(
                    fSqlSetting.SqlExecuting,
                    fSqlSetting.SqlExecuted);
            }

            var fSql = freeSqlBuilder.Build();


            // Aop配置实体
            if (fSqlSetting.ConfigEntity != null)
            {
                fSql.Aop.ConfigEntity += new EventHandler<FreeSql.Aop.ConfigEntityEventArgs>(fSqlSetting.ConfigEntity);
            }


            // Aop配置实体属性
            if (fSqlSetting.ConfigEntityProperty != null)
            {
                fSql.Aop.ConfigEntityProperty += new EventHandler<FreeSql.Aop.ConfigEntityPropertyEventArgs>(fSqlSetting.ConfigEntityProperty);
            }

            return fSql;
        }

        /// <summary>
        /// 获取数据库类型
        /// </summary>
        /// <param name="databaseType"></param>
        /// <returns></returns>
        private FreeSql.DataType GetDbType(DatabaseType databaseType)
        {
            switch (databaseType)
            {
                case DatabaseType.SqlServer:
                    return FreeSql.DataType.SqlServer;
                case DatabaseType.PostgreSql:
                    return FreeSql.DataType.PostgreSQL;
                case DatabaseType.Oracle:
                    return FreeSql.DataType.Oracle;
                case DatabaseType.Sqlite:
                    return FreeSql.DataType.Sqlite;
            }

            throw new ArgumentException("Invalid database type");
        }


        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            this.FSql?.Dispose();
        }
    }
}
