using System;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Abp.Data;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Repositories;
using Yoyo.Pro.Common;
using Microsoft.EntityFrameworkCore;

namespace Yoyo.Pro.Repositories
{

    /// <summary>
    /// YoyoPro实现的泛型仓储
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public class YoyoProEfCoreRepository<TEntity, TPrimaryKey> : EfCoreRepositoryBase<DbContext, TEntity, TPrimaryKey>,
        ISupportsExplicitLoading<TEntity, TPrimaryKey>,
        IRepositoryWithDbContext
        where TEntity : class, IEntity<TPrimaryKey>

    {
        protected readonly IYoyoProDbContextProvider _dbContextProvider;
        protected readonly IActiveTransactionProvider _transactionProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContextProvider"></param>
        public YoyoProEfCoreRepository(IServiceProvider serviceProvider)
            : base(null)
        {
            this._dbContextProvider = serviceProvider
                .GetService(typeof(IYoyoProDbContextProvider)) as IYoyoProDbContextProvider;
            this._transactionProvider = serviceProvider
                .GetService(typeof(IActiveTransactionProvider)) as IActiveTransactionProvider;
        }

        protected override IQueryable<TEntity> GetQueryable()
        {
            return GetTable().AsQueryable();
        }

        protected override async Task<IQueryable<TEntity>> GetQueryableAsync()
        {
            return (await GetTableAsync()).AsQueryable();
        }

        public override DbContext GetContext()
        {
            return _dbContextProvider.GetDbContext(MultiTenancySide);
        }

        public override Task<DbContext> GetContextAsync()
        {
            return _dbContextProvider.GetDbContextAsync(MultiTenancySide);
        }

        public override DbTransaction GetTransaction()
        {
            return (DbTransaction)this._transactionProvider?.GetActiveTransaction(new ActiveTransactionProviderArgs
            {
                {"ContextType",  _dbContextProvider.DbContextType},
                {"MultiTenancySide", MultiTenancySide}
            });
        }

        public override async Task<DbTransaction> GetTransactionAsync()
        {
            if (this._transactionProvider == null)
            {
                return null;
            }

            var transaction = await this._transactionProvider.GetActiveTransactionAsync(new ActiveTransactionProviderArgs
            {
                {"ContextType", _dbContextProvider.DbContextType},
                {"MultiTenancySide", MultiTenancySide}
            });

            return (DbTransaction)transaction;
        }
    }




    /// <summary>
    /// YoyoPro实现的泛型仓储
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class YoyoProEfCoreRepository<TEntity> : YoyoProEfCoreRepository<TEntity, string>, IRepository<TEntity>
      where TEntity : class, IEntity<string>
    {
        public YoyoProEfCoreRepository(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }
    }

}
