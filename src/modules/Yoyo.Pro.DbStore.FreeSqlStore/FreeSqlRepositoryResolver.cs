using System;
using FreeSql;
using Yoyo.Pro.DbStore.FreeSqlStore.Partition;
using Yoyo.Pro.DbStore.FreeSqlStore.Repositories;
using Yoyo.Pro.DbStore.Partition;
using Yoyo.Pro.DbStore.Repositories;

namespace Yoyo.Pro.DbStore.FreeSqlStore
{
    public class FreeSqlRepositoryResolver
    {


        protected readonly IServiceProvider ServiceProvider;

        protected readonly IFSqlProviderDbStore FsqlProviderDbStore;

        public IRelationalDatabaseProcessorDbStore RelationalDatabaseProcessorDbStore { get; private set; }

        public IPartitionTableNameFactory PartitionTableNameFactory { get; private set; }


        public FreeSqlRepositoryResolver(IServiceProvider serviceProvider, IFSqlProviderDbStore fsqlProviderDbStore, IRelationalDatabaseProcessorDbStore relationalDatabaseProcessorDbStore, IPartitionTableNameFactory partitionTableNameFactory)
        {
            ServiceProvider = serviceProvider;
            FsqlProviderDbStore = fsqlProviderDbStore;
            RelationalDatabaseProcessorDbStore = relationalDatabaseProcessorDbStore;
            PartitionTableNameFactory = partitionTableNameFactory;
        }

        /// <summary>
        /// 获取FreeSql对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IFreeSql GetFreeSql(string name = null)
        {
            return this.FsqlProviderDbStore.GetByName(name,  FreeSqlDbStoreConsts.DefaultProviderName).FSql;
        }


        /// <summary>
        /// 获取仓储
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TPrimaryKey"></typeparam>
        /// <param name="partitionId"></param>
        /// <param name="freeSqlName"></param>
        /// <returns></returns>
        public FSqlPartitionRepository<TEntity, TPrimaryKey> GetRepository<TEntity, TPrimaryKey>(object partitionId, string freeSqlName = null)
            where TEntity : class
        {
            var entityName = typeof(TEntity).Name;
            var tableName = this.PartitionTableNameFactory.GetTableName(entityName, partitionId);


            var freeSql = GetFreeSql(freeSqlName);

            var repository = new FSqlPartitionRepository<TEntity, TPrimaryKey>(freeSql, null, (oldName) =>
            {
                return RelationalDatabaseProcessorDbStore.GetByName(freeSqlName, FreeSqlDbStoreConsts.DefaultProviderName).HandleString(tableName);
            });

            return repository;
        }

        /// <summary>
        /// 获取仓储
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TPrimaryKey"></typeparam>
        /// <param name="partitionId"></param>
        /// <param name="freeSqlName"></param>
        /// <param name="unitOfWork"></param>
        /// <returns></returns>
        // ReSharper disable once MethodOverloadWithOptionalParameter
        public FSqlPartitionRepository<TEntity, TPrimaryKey> GetRepository<TEntity, TPrimaryKey>(object partitionId, string freeSqlName = null, IUnitOfWork unitOfWork = null)
              where TEntity : class
        {
            if (unitOfWork == null)
            {
                return this.GetRepository<TEntity, TPrimaryKey>(partitionId, freeSqlName);
            }


            var entityName = typeof(TEntity).Name;
            var tableName = this.PartitionTableNameFactory.GetTableName(entityName, partitionId);

            var freeSql = this.GetFreeSql(freeSqlName);

            var repository = new FSqlPartitionRepository<TEntity, TPrimaryKey>(freeSql, unitOfWork, null, (oldName) =>
            {
                return this.RelationalDatabaseProcessorDbStore.GetByName(freeSqlName, FreeSqlDbStoreConsts.DefaultProviderName).HandleString(tableName);
            });

            return repository;
        }
    }

}
