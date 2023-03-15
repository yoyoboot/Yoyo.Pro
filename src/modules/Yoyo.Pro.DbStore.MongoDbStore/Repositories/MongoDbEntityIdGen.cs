using MongoDB.Bson;
using Yoyo.Pro.DbStore.Repositories;

namespace Yoyo.Pro.DbStore.MongoDbStore.Repositories
{
    public class MongoDbEntityIdGen : IDbEntityIdGen
    {
        public string Next => ObjectId.GenerateNewId().ToString();

    }
}
