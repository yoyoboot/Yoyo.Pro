using System;
using System.Collections.Generic;
using System.Linq;
using Yoyo.Pro.DbStore.Repositories;
using Yoyo.Pro.DbStore.Uow;

namespace Yoyo.Pro.DbStore.Switcher.Repositories
{
    public abstract class BaseDbRepository<TEntity, TPrimaryKey> : IDbRepository<TEntity, TPrimaryKey>
             where TEntity : class, IDbEntity<TPrimaryKey>
    {
        protected IDbRepository<TEntity, TPrimaryKey> DbStorage { get; set; }

        protected IDbRepositoryFactory StorageFactory { get; }

        protected IServiceProvider ServiceProvider { get; }

        protected DatabaseType DatabaseTypeChange { get; set; } = DatabaseType.MongoDB;

        protected BaseDbRepository(IDbRepositoryFactory storageFactory, IServiceProvider serviceProvider = null)
        {
            StorageFactory = storageFactory;
            ServiceProvider = serviceProvider;

            // ReSharper disable once VirtualMemberCallInConstructor
            DbStorage = this.CreateDbStorage(StorageFactory);
        }


        protected abstract IDbRepository<TEntity, TPrimaryKey> CreateDbStorage(IDbRepositoryFactory storageFactory);


        public IDbUnitOfWork BeginUnitOfWork()
        {
            return DbStorage.BeginUnitOfWork();
        }

        public IDisposable ChangeProvider(string name, DatabaseType databaseType)
        {
            DatabaseTypeChange = databaseType;
            DbStorage = CreateDbStorage(StorageFactory);
            return DbStorage.ChangeProvider(name, databaseType);
        }

        public long Count(object partitionId = null)
        {
            return DbStorage.Count(partitionId);
        }

        public void Delete(TPrimaryKey id, object partitionId = null)
        {
            DbStorage.Delete(id, partitionId);
        }

        public void Delete(List<TPrimaryKey> ids, object partitionId = null)
        {
            DbStorage.Delete(ids, partitionId);
        }

        public void Dispose()
        {
            DbStorage.Dispose();
        }

        public TEntity Get(TPrimaryKey id, object partitionId = null)
        {
            return DbStorage.Get(id, partitionId);
        }

        public IDbUnitOfWork GetCurrentUnitOfWork()
        {
            return DbStorage.GetCurrentUnitOfWork();
        }

        public IQueryable<TEntity> GetQuery(object partitionId = null)
        {
            return DbStorage.GetQuery(partitionId);
        }

        public TEntity Insert(TEntity entity, object partitionId = null)
        {
            return DbStorage.Insert(entity, partitionId);
        }

        public void Insert(List<TEntity> entitys, object partitionId = null)
        {
            DbStorage.Insert(entitys, partitionId);
        }

        public void Rollback()
        {
            DbStorage.Rollback();
        }

        public void SaveChanges()
        {
            DbStorage.SaveChanges();
        }

        public TEntity Update(TEntity entity, object partitionId = null)
        {
            return DbStorage.Update(entity, partitionId);
        }

        public void Update(List<TEntity> entitys, object partitionId = null)
        {
            DbStorage.Update(entitys, partitionId);
        }

        public void BatchInsert(ICollection<TEntity> entities, object partitionId)
        {
            this.DbStorage.BatchInsert(entities, partitionId);
        }

    }
}
