using MongoDB.Driver;

namespace Yoyo.Pro.DbStore.MongoDbStore
{
    public class MongoTableWrapper<TEntity>
    {
        public IMongoCollection<TEntity> Table { get; }

        public IClientSessionHandle ClientSession { get; }

        public MongoTableWrapper(IMongoCollection<TEntity> table)
        {
            this.Table = table;
        }

        public MongoTableWrapper(IMongoCollection<TEntity> table, IClientSessionHandle clientSession)
            : this(table)
        {
            this.ClientSession = clientSession;
        }
    }
}
