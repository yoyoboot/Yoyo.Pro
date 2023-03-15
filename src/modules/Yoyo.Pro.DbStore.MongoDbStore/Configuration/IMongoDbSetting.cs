using MongoDB.Driver;

namespace Yoyo.Pro.DbStore.MongoDbStore.Configuration
{
    /// <summary>
    /// 数据库配置 (单例)
    /// </summary>
    public interface IMongoDbSetting
    {
        /// <summary>
        /// 配置名
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 连接字符串
        /// </summary>
        string ConnectionString { get; set; }

        /// <summary>
        /// 默认数据库名称
        /// </summary>
        string DatabaseName { get; set; }

        /// <summary>
        /// 默认数据库配置
        /// </summary>
        MongoDatabaseSettings DatabaseSettings { get; set; }
    }
}
