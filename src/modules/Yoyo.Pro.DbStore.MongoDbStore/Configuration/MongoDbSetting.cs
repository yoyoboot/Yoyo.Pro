using MongoDB.Driver;

namespace Yoyo.Pro.DbStore.MongoDbStore.Configuration
{
    public class MongoDbSetting : IMongoDbSetting
    {
        public virtual string Name { get; set; }
        public virtual string ConnectionString { get; set; }
        public virtual string DatabaseName { get; set; }
        public virtual MongoDatabaseSettings DatabaseSettings { get; set; }
    }
}
