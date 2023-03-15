using Yoyo.Pro.DbStore.MongoDbStore.Repositories;
using Yoyo.Pro.DbStore.Repositories;

namespace Yoyo.Pro.DbStore.MongoDbStore
{
    public static class MongoDbRepositoryFactoryExtensions
    {
        /// <summary>
        /// 获取 mongodb 实体仓储,默认值 MongoDbStoreConsts.DefaultMongoRepositoryName
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TPrimaryKey">实体主键</typeparam>
        /// <param name="dbRepositoryFactory">仓储工厂</param>
        /// <returns>返回仓储实例</returns>
        public static IMongoDbRepository<TEntity, TPrimaryKey> GetDefaultIMongoRepository<TEntity, TPrimaryKey>(this IDbRepositoryFactory dbRepositoryFactory)
             where TEntity : class, IDbEntity<TPrimaryKey>
        {
            return (IMongoDbRepository<TEntity, TPrimaryKey>)dbRepositoryFactory.GetDbRepository<TEntity, TPrimaryKey>(MongoDbStoreConsts.DefaultMongoRepositoryName);
        }

        /// <summary>
        /// 获取 mongodb 实体仓储
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TPrimaryKey">实体主键</typeparam>
        /// <param name="dbRepositoryFactory">仓储工厂</param>
        /// <param name="mongoRepositoryName">仓储类型映射名称</param>
        /// <returns>返回仓储实例</returns>
        public static IMongoDbRepository<TEntity, TPrimaryKey> GetIMongoRepository<TEntity, TPrimaryKey>(this IDbRepositoryFactory dbRepositoryFactory, string mongoRepositoryName)
            where TEntity : class, IDbEntity<TPrimaryKey>
        {
            return (IMongoDbRepository<TEntity, TPrimaryKey>)dbRepositoryFactory.GetDbRepository<TEntity, TPrimaryKey>(mongoRepositoryName);
        }
    }
}
