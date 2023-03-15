using MongoDB.Driver;

namespace Yoyo.Pro.DbStore.MongoDbStore.Partition
{
    public interface IMongoClientProvider
    {
        /// <summary>
        /// 配置提供名称
        /// </summary>
        string ProviderName { get; }

        /// <summary>
        /// 数据库名称
        /// </summary>
        string DatabaseName { get; }

        /// <summary>
        /// 客户端
        /// </summary>
        IMongoClient Client { get; }

        /// <summary>
        /// 数据库配置
        /// </summary>
        MongoDatabaseSettings DatabaseSettings { get;  }
    }
}
