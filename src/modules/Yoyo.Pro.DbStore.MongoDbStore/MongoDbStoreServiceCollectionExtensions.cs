using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Yoyo.Pro.DbStore.MongoDbStore.Configuration;
using Yoyo.Pro.DbStore.MongoDbStore.Partition;
using Yoyo.Pro.DbStore.MongoDbStore.Repositories;
using Yoyo.Pro.DbStore.Repositories;

namespace Yoyo.Pro.DbStore.MongoDbStore
{
    public static class MongoDbStoreServiceCollectionExtensions
    {
        /// <summary>
        /// 添加MongoDb支持
        /// </summary>
        /// <param name="services">服务注册集合</param>
        /// <param name="defaultDbSetting">默认的数据库连接配置信息</param>
        /// <param name="useMongoDbEntityIdGen">使用mongodb的id生成器</param>
        /// <returns></returns>
        public static IServiceCollection AddMongoDbStore(this IServiceCollection services,
            IMongoDbSetting defaultDbSetting, bool useMongoDbEntityIdGen = true)
        {
            if (defaultDbSetting == null)
            {
                throw new ArgumentNullException(nameof(defaultDbSetting));
            }
            services.AddTransient<MongoDbRepositoryResolver>();

            services.AddMongoDatabaseProvider(defaultDbSetting);

            services.TryAddTransient(typeof(IMongoDbRepository<,>), typeof(MongoDbRepository<,>));
            services.TryAddTransient(typeof(IDbRepository<,>), typeof(MongoDbRepository<,>));

            if (useMongoDbEntityIdGen)
            {
                var descriptor = new ServiceDescriptor(typeof(IDbEntityIdGen), typeof(MongoDbEntityIdGen), ServiceLifetime.Singleton);
                services.Replace(descriptor);
            }

            services.TryAddSingleton<IDbEntityIdGen, MongoDbEntityIdGen>();

            return services;
        }

        /// <summary>
        /// 添加新的数据库配置
        /// </summary>
        /// <param name="services">服务注册集合</param>
        /// <param name="dbSetting">数据库配置信息</param>
        /// <returns></returns>
        public static IServiceCollection AddMongoDatabaseProvider(this IServiceCollection services, IMongoDbSetting dbSetting)
        {
            if (dbSetting == null)
            {
                throw new ArgumentNullException(nameof(dbSetting));
            }
            // 添加对应的关系型数据库配置
            services.AddRelationalDatabaseProcessor(
                typeof(IMongoClientProvider),
                DatabaseType.MongoDB,
                dbSetting.Name
            );

            services.AddSingleton<IMongoClientProvider>(new MongoClientProvider(dbSetting));

            return services;
        }


        /// <summary>
        /// 添加默认仓储类型映射
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static IDbRepositoryDict UseDefaultMongoRepositoryMap(this IServiceProvider serviceProvider)
        {
            var dbRepositoryDict = serviceProvider.GetService<IDbRepositoryDict>();

            // ReSharper disable once PossibleNullReferenceException
            dbRepositoryDict.Add(MongoDbStoreConsts.DefaultMongoRepositoryName, typeof(IMongoDbRepository<,>));

            return dbRepositoryDict;
        }

        /// <summary>
        /// 添加默认仓储类型映射
        /// </summary>
        /// <param name="dbRepositoryDict"></param>
        /// <returns></returns>
        public static IDbRepositoryDict UseDefaultMongoRepositoryMap(this IDbRepositoryDict dbRepositoryDict)
        {
            dbRepositoryDict.Add(MongoDbStoreConsts.DefaultMongoRepositoryName, typeof(IMongoDbRepository<,>));

            return dbRepositoryDict;
        }
    }
}
