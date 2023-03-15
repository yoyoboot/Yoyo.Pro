using MongoDB.Driver;
using Yoyo.Pro.DbStore.MongoDbStore.Configuration;

namespace Yoyo.Pro.DbStore.MongoDbStore.Partition
{
    public class MongoClientProvider : IMongoClientProvider
    {
        protected readonly IMongoDbSetting Setting;

        public string ProviderName => Setting.Name;

        public string DatabaseName => Setting.DatabaseName;

        public IMongoClient Client { get; private set; }

        public MongoDatabaseSettings DatabaseSettings => Setting.DatabaseSettings;

        public MongoClientProvider(IMongoDbSetting setting)
        {
            this.Setting = setting;

            Client = new MongoClient(this.Setting.ConnectionString);
        }
    }
}
