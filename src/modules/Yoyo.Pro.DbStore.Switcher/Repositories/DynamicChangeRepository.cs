using System;
using Yoyo.Pro.DbStore.FreeSqlStore;
using Yoyo.Pro.DbStore.MongoDbStore;
using Yoyo.Pro.DbStore.Repositories;

namespace Yoyo.Pro.DbStore.Switcher.Repositories
{
    public class DynamicChangeRepository<TEntity, TPrimaryKey> : BaseDbRepository<TEntity, TPrimaryKey>,
        IDynamicChangeRepository<TEntity, TPrimaryKey>
        where TEntity : class, IDbEntity<TPrimaryKey>
    {
        public IDbEntityIdGen EntityIdGen { get; }

        public IDbRepository<TEntity, TPrimaryKey> DbRepository => DbStorage;

        public DynamicChangeRepository(IDbRepositoryFactory storageFactory, IDbEntityIdGen dbEntityIdGen, IServiceProvider serviceProvider)
            : base(storageFactory, serviceProvider)
        {
            this.EntityIdGen = dbEntityIdGen;
        }

        protected override IDbRepository<TEntity, TPrimaryKey> CreateDbStorage(IDbRepositoryFactory storageFactory)
        {
            IDbRepository<TEntity, TPrimaryKey> repo;

            switch (DatabaseTypeChange)
            {
                // ReSharper disable once UnreachableSwitchCaseDueToIntegerAnalysis
                case DatabaseType.MongoDB:
                    repo = storageFactory.GetDefaultIMongoRepository<TEntity, TPrimaryKey>();
                    break;
                    
                default:
                    repo = storageFactory.GetDefaultIFreeRepository<TEntity, TPrimaryKey>();
                    break;
            }

            return repo;
        }


        public new void Dispose()
        {
            DbStorage.Dispose();
        }

    }

}
