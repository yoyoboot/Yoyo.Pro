using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using Yoyo.Pro.DbStore.MongoDbStore.Uow;
using Yoyo.Pro.DbStore.Partition;
using Yoyo.Pro.DbStore.Repositories;
using Yoyo.Pro.DbStore.Uow;

namespace Yoyo.Pro.DbStore.MongoDbStore.Repositories
{
    public class MongoDbRepository<TEntity, TPrimaryKey> : IMongoDbRepository<TEntity, TPrimaryKey>
         where TEntity : class, IDbEntity<TPrimaryKey>
    {
        public const string DefaultProviderName = "MongoDefault";

        protected readonly string EntityName;
        protected readonly MongoDbRepositoryResolver MongoDbRepositoryResolver;
        private readonly Dictionary<string, MongoDbUnitOfWork> _unitOfWorkDict;
        private readonly Dictionary<string, MongoUnitOfWorkOptions> _unitOfWorkOptionsDict;


        public string ProviderName { get; private set; }
        public string OldProviderName { get; private set; }


        MongoDbRepositoryResolver RepositoryResolver
            => this.MongoDbRepositoryResolver;
        IPartitionTableNameFactory PartitionTableNameFactory
            => this.MongoDbRepositoryResolver.PartitionTableNameFactory;
        IRelationalDatabaseProcessor RelationalDatabaseProcessor
            => this.MongoDbRepositoryResolver.RelationalDatabaseProcessor;


        public MongoDbRepository(MongoDbRepositoryResolver mongoDbRepositoryResolver)
        {
            MongoDbRepositoryResolver = mongoDbRepositoryResolver;

            EntityName = typeof(TEntity).Name;
            _unitOfWorkDict = new Dictionary<string, MongoDbUnitOfWork>();
            _unitOfWorkOptionsDict = new Dictionary<string, MongoUnitOfWorkOptions>();
        }


        #region 公开,修改数据库,获取database,启动事务,提交事务，获取当前事务

        /// <inheritdoc/>
        public IDisposable ChangeProvider(string name, DatabaseType databaseType)
        {
            OldProviderName = ProviderName;
            ProviderName = name;

            return new DisposeAction(() =>
            {
                ProviderName = OldProviderName;
                OldProviderName = null;
            });
        }

        /// <inheritdoc/>
        public IMongoDatabase GetDatabase()
        {
            return this.RepositoryResolver.GetMongoDatabase(this.ProviderName, this.GetCurrentMongoUnitOfWork()?.Client);
        }

        /// <inheritdoc/>
        public virtual IMongoCollection<TEntity> GetTable(object partitionId)
        {
            var currentUnitOfWork = this.GetCurrentMongoUnitOfWork();

            var table = this.RepositoryResolver.GetTable<TEntity>(partitionId, this.ProviderName, currentUnitOfWork?.Client);

            var weakReference = new WeakReference(table);

            return weakReference.Target as IMongoCollection<TEntity>;
        }

        /// <inheritdoc/>
        public IDbUnitOfWork BeginUnitOfWork()
        {
            return this.BeginUnitOfWork(null);
        }

        /// <inheritdoc/>
        public IDbUnitOfWork BeginUnitOfWork(MongoUnitOfWorkOptions unitOfWorkOptions)
        {
            var currentUnitOfWork = this.GetCurrentUnitOfWork();
            if (currentUnitOfWork != null)
            {
                return currentUnitOfWork;
            }

            var unitOfWorkKey = this.GetUnitOfWorkKey();

            // 缓存工作单元配置
            this._unitOfWorkOptionsDict[unitOfWorkKey] = unitOfWorkOptions;

            // 获取provider
            var provider = this.RepositoryResolver.GetMongoClientProvider(this.ProviderName);

            // 创建工作单元
            var clientSessionHandle = provider.Client.StartSession(unitOfWorkOptions?.ClientSessionOptions);
            clientSessionHandle.StartTransaction(unitOfWorkOptions?.TransactionOptions);
            var mongoDbUnitOfWork = new MongoDbUnitOfWork(clientSessionHandle, () =>
            {
                this.RemoveUnitOfWork(unitOfWorkKey);
            });

            // 缓存工作单元
            this._unitOfWorkDict[unitOfWorkKey] = mongoDbUnitOfWork;

            return mongoDbUnitOfWork;
        }

        /// <inheritdoc/>
        public IDbUnitOfWork GetCurrentUnitOfWork()
        {
            var unitOfWorkKey = this.GetUnitOfWorkKey();
            if (this._unitOfWorkDict.TryGetValue(unitOfWorkKey, out MongoDbUnitOfWork unitOfWork))
            {
                return unitOfWork;
            }

            return null;
        }




        /// <inheritdoc/>
        public void SaveChanges()
        {
            this.GetCurrentUnitOfWork()?.Commit();
        }

        public void Rollback()
        {
            this.GetCurrentUnitOfWork()?.Rollback();
        }


        /// <inheritdoc/>
        public void Dispose()
        {
            foreach (var item in this._unitOfWorkDict)
            {
                item.Value?.Dispose();
            }
            this._unitOfWorkDict.Clear();
            this._unitOfWorkOptionsDict.Clear();
        }

        #endregion



        #region 增删改查函数

        /// <inheritdoc/>
        public IQueryable<TEntity> GetQuery(object partitionId = null)
        {
            return this.GetTable(partitionId).AsQueryable();
        }

        /// <inheritdoc/>
        public TEntity Get(TPrimaryKey id, object partitionId = null)
        {
            return this.GetTable(partitionId).Find(GetPrimaryFilter(id)).FirstOrDefault();
        }

        /// <inheritdoc/>
        public TEntity Insert(TEntity entity, object partitionId = null)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            // ReSharper disable once PossibleNullReferenceException
            if (entity is IPartitionEntity && partitionId != null)
            {
                // ReSharper disable once SuspiciousTypeConversion.Global
                // ReSharper disable once PossibleNullReferenceException
                ((IPartitionEntity)entity).PartitionId = partitionId;
                var table = this.GetTable(partitionId);
                table.InsertOne(entity);
            }
            // ReSharper disable once SuspiciousTypeConversion.Global
            // ReSharper disable once PossibleNullReferenceException
            else if (entity is IPartitionEntity)
            {                
                // ReSharper disable once SuspiciousTypeConversion.Global
                // ReSharper disable once PossibleNullReferenceException
                var table = this.GetTable(((IPartitionEntity)entity).PartitionId);
                table.InsertOne(entity);
            }
            else
            {
                this.GetTable(null).InsertOne(entity);
            }
            return entity;
        }

        /// <inheritdoc/>
        public void Insert(List<TEntity> entitys, object partitionId = null)
        {
            foreach (var entity in entitys)
            {
                this.Insert(entity, partitionId);
            }
        }

        /// <inheritdoc/>
        public TEntity Update(TEntity entity, object partitionId = null)
        {                
            // ReSharper disable once SuspiciousTypeConversion.Global
            // ReSharper disable once PossibleNullReferenceException
            if (entity is IPartitionEntity && partitionId != null)
            {
                // ReSharper disable once SuspiciousTypeConversion.Global
                // ReSharper disable once PossibleNullReferenceException
                ((IPartitionEntity)entity).PartitionId = partitionId;
                var table = this.GetTable(partitionId);
                table.ReplaceOne(GetPrimaryFilter(entity.Id), entity);
            }                
            // ReSharper disable once SuspiciousTypeConversion.Global
            // ReSharper disable once PossibleNullReferenceException
            else if (entity is IPartitionEntity)
            {                // ReSharper disable once SuspiciousTypeConversion.Global
                // ReSharper disable once PossibleNullReferenceException
                var table = this.GetTable(((IPartitionEntity)entity).PartitionId);
                table.ReplaceOne(GetPrimaryFilter(entity.Id), entity);
            }
            else
            {
                this.GetTable(null).ReplaceOne(GetPrimaryFilter(entity.Id), entity);
            }

            return entity;
        }

        /// <inheritdoc/>
        public void Update(List<TEntity> entitys, object partitionId = null)
        {
            foreach (var entity in entitys)
            {
                this.Update(entity, partitionId);
            }
        }

        /// <inheritdoc/>
        public void Delete(TPrimaryKey id, object partitionId = null)
        {
            var table = GetTable(partitionId);
            table.DeleteOne(GetPrimaryFilter(id));
        }

        /// <inheritdoc/>
        public void Delete(List<TPrimaryKey> ids, object partitionId = null)
        {
            var table = GetTable(partitionId);
            foreach (var id in ids)
            {
                table.DeleteOne(GetPrimaryFilter(id));
            }
        }

        /// <inheritdoc/>
        public long Count(object partitionId = null)
        {
            return GetQuery(partitionId).LongCount();
        }

        #endregion



        #region 辅助函数

        /// <summary>
        /// 获取主键的过滤条件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private static FilterDefinition<TEntity> GetPrimaryFilter(TPrimaryKey id)
        {
            return Builders<TEntity>.Filter.Eq(e => e.Id, id);
        }

        /// <summary>
        /// 获取表缓存键值
        /// </summary>
        /// <param name="partitionId"></param>
        /// <returns></returns>
        // ReSharper disable once UnusedMember.Local
        private string GetTableKey(object partitionId = null)
        {
            var tableName = this.RelationalDatabaseProcessor.HandleString(
                   this.PartitionTableNameFactory.GetTableName(this.EntityName, partitionId)
                  );

            return $"{this.GetUnitOfWorkKey()}-{tableName}";
        }

        /// <summary>
        /// 获取工作单元缓存键值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string GetUnitOfWorkKey(string name = null)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                return name;
            }

            return string.IsNullOrWhiteSpace(this.ProviderName) ? DefaultProviderName : this.ProviderName;
        }

        /// <summary>
        /// 释放工作单元,默认是当前
        /// </summary>
        /// <param name="unitOfWorkKey"></param>
        private void RemoveUnitOfWork(string unitOfWorkKey = null)
        {
            unitOfWorkKey = unitOfWorkKey ?? this.GetUnitOfWorkKey();

            // 释放工作单元
            if (this._unitOfWorkDict.TryGetValue(unitOfWorkKey, out MongoDbUnitOfWork unitOfWork))
            {
                unitOfWork?.Dispose();
                this._unitOfWorkDict.Remove(unitOfWorkKey);
            }

            // 移除工作单元配置缓存
            if (this._unitOfWorkOptionsDict.TryGetValue(unitOfWorkKey, out _))
            {
                this._unitOfWorkOptionsDict.Remove(unitOfWorkKey);
            }
        }




        /// <summary>
        /// 获取 mongodb 的工作单元
        /// </summary>
        /// <returns></returns>
        private IClientSessionHandle GetCurrentMongoUnitOfWork()
        {
            var dbUnitOfWork = this.GetCurrentUnitOfWork();

            return (dbUnitOfWork as MongoDbUnitOfWork)?.Outer;
        }

        public void BatchInsert(ICollection<TEntity> entities, object partitionId)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }
            if (partitionId == null)
            {
                throw new ArgumentNullException(nameof(partitionId));
            }
            // ReSharper disable once SuspiciousTypeConversion.Global
            // ReSharper disable once PossibleNullReferenceException
            if (entities.Any(o => !(o is IPartitionEntity)))
            {
                throw new ArgumentException($"The data entity does not implement the IPartitionEntity interface");
            }

            foreach (var entity in entities)
            {
                // ReSharper disable once SuspiciousTypeConversion.Global
                // ReSharper disable once PossibleNullReferenceException
                (entity as IPartitionEntity).PartitionId = partitionId;
            }

            this.GetTable(partitionId).InsertMany(entities, new InsertManyOptions()
            {
                BypassDocumentValidation = false,
            });
        }

        #endregion
    }
}
