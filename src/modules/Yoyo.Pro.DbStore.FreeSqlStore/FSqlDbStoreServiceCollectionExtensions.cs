using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Yoyo.Pro.DbStore;
using Yoyo.Pro.DbStore.FreeSqlStore.Configuration;
using Yoyo.Pro.DbStore.FreeSqlStore.Partition;
using Yoyo.Pro.DbStore.FreeSqlStore.Repositories;
using Yoyo.Pro.DbStore.Repositories;

namespace Yoyo.Pro.DbStore.FreeSqlStore
{
    public static class FSqlDbStoreServiceCollectionExtensions
    {
        /// <summary>
        /// 添加FreeSql支持
        /// </summary>
        /// <param name="services"></param>
        /// <param name="defaultDbSetting"></param>
        /// <returns></returns>
        public static IServiceCollection AddFreeSqlDbStore(this IServiceCollection services,
            IFSqlSetting defaultDbSetting)
        {

            if (defaultDbSetting == null)
            {
                throw new ArgumentNullException(nameof(defaultDbSetting));
            }

            services.AddFreeSqlDatabaseProvider(defaultDbSetting);

            services.AddTransient<FreeSqlRepositoryResolver>();
            services.TryAddTransient(typeof(IFSqlRepository<,>), typeof(FSqlRepository<,>));
            services.TryAddTransient(typeof(IDbRepository<,>), typeof(FSqlRepository<,>));

            services.TryAddSingleton<IFSqlProviderDbStore, DefaultIfSqlProviderDbStore>();

            return services;
        }


        /// <summary>
        /// 添加新的 freeSql 数据库配置
        /// </summary>
        /// <param name="services">服务注册集合</param>
        /// <param name="dbSetting">数据库配置信息</param>
        /// <returns></returns>
        public static IServiceCollection AddFreeSqlDatabaseProvider(this IServiceCollection services, IFSqlSetting dbSetting)
        {
            if (dbSetting == null)
            {
                throw new ArgumentNullException(nameof(dbSetting));
            }


            services.AddSingleton<IFSqlProvider>((serviceProvider) =>
            {
                return new FSqlProvider(dbSetting);
            });

            // 添加对应的关系型数据库配置
            services.AddRelationalDatabaseProcessor(
                typeof(IFSqlProvider),
                dbSetting.DatabaseType,
                dbSetting.Name
            );

            return services;
        }

        public static IServiceProvider AddFreeSqlDatabaseProvider(this IServiceProvider serviceProvider, IFSqlSetting dbSetting)
        {
            if (dbSetting == null)
            {
                throw new ArgumentNullException(nameof(dbSetting));
            }

            var fSqlProviderStorage = serviceProvider.GetRequiredService<IFSqlProviderDbStore>();

            fSqlProviderStorage.AddOrUpdate(dbSetting.Name, new FSqlProvider(dbSetting));



            serviceProvider.AddRelationalDatabaseProcessor(
                    typeof(IFSqlProvider),
                    dbSetting.DatabaseType,
                    dbSetting.Name
                );

            return serviceProvider;
        }


    }
}
