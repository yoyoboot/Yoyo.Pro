using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Yoyo.Pro.DbStore.MongoDbStore.Partition;
using Yoyo.Pro.DbStore.Partition;
using Yoyo.Pro.DbStore.Repositories;

namespace Yoyo.Pro.DbStore.MongoDbStore
{
    public class MongoDbRepositoryResolver
    {
        protected readonly Dictionary<string, IMongoClientProvider> ProviderName2DatabaseProvider;

        protected readonly IServiceProvider ServiceProvider;

        public IRelationalDatabaseProcessor RelationalDatabaseProcessor { get; private set; }
        public IPartitionTableNameFactory PartitionTableNameFactory { get; private set; }

        public MongoDbRepositoryResolver(IServiceProvider serviceProvider, IRelationalDatabaseProcessor relationalDatabaseProcessor, IPartitionTableNameFactory partitionTableNameFactory)
        {
            ServiceProvider = serviceProvider;
            RelationalDatabaseProcessor = relationalDatabaseProcessor;
            PartitionTableNameFactory = partitionTableNameFactory;


            this.ProviderName2DatabaseProvider = ServiceProvider.GetServices<IMongoClientProvider>().ToDictionary(o => o.ProviderName);
        }

        /// <summary>
        /// 获取 IMongoClientProvider 对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IMongoClientProvider GetMongoClientProvider(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                var mongoDatabaseProvider = ProviderName2DatabaseProvider.Values.FirstOrDefault();
                if (mongoDatabaseProvider == null)
                {
                    throw new Exception("Unregistered MongoDb default configuration");
                }

                return mongoDatabaseProvider;
            }


            if (!ProviderName2DatabaseProvider.TryGetValue(name, out IMongoClientProvider provider))
            {
                throw new ArgumentException($"The MongoDb Provider with the name {name} was not found");
            }
            return provider;
        }

        /// <summary>
        /// 获取 MongoDatabase
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IMongoDatabase GetMongoDatabase(string name)
        {
            var provider = this.GetMongoClientProvider(name);

            return provider.Client.GetDatabase(provider.DatabaseName, provider.DatabaseSettings);
        }

        /// <summary>
        /// 获取 MongoDatabase,通过已存在的Client
        /// </summary>
        /// <param name="name"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public IMongoDatabase GetMongoDatabase(string name, IMongoClient client)
        {
            if (client == null)
            {
                return this.GetMongoDatabase(name);
            }


            var provider = this.GetMongoClientProvider(name);
            return client.GetDatabase(provider.DatabaseName, provider.DatabaseSettings);
        }


        /// <summary>
        /// 获取表
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="partitionId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public IMongoCollection<TEntity> GetTable<TEntity>(object partitionId, string name = null)
        {
            var entityName = typeof(TEntity).Name;
            var tableName = this.PartitionTableNameFactory.GetTableName(entityName, partitionId);

            return this.GetMongoDatabase(name).GetCollection<TEntity>(tableName);
        }

        /// <summary>
        /// 获取表
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="partitionId"></param>
        /// <param name="name"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        // ReSharper disable once MethodOverloadWithOptionalParameter
        public IMongoCollection<TEntity> GetTable<TEntity>(object partitionId, string name = null, IMongoClient client = null)
        {
            var entityName = typeof(TEntity).Name;
            var tableName = this.PartitionTableNameFactory.GetTableName(entityName, partitionId);

            return this.GetMongoDatabase(name, client).GetCollection<TEntity>(tableName);
        }

    }
}
