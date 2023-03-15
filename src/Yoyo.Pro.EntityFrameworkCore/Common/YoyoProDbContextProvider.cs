using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore;
using Abp.MultiTenancy;
using Yoyo.Pro.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Yoyo.Pro.Common
{
    public class YoyoProDbContextProvider : IYoyoProDbContextProvider
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly IYoyoProDbContextTypeStorage _dbContextTypeStorage;
        protected readonly ICurrentUnitOfWorkProvider _currentUnitOfWorkProvider;

        public virtual Type DbContextType { get; protected set; }

        protected virtual string Name { get; set; }

        protected virtual object DbContextProvider { get; set; }

        protected virtual PropertyInfo TaskDbContextTypeResult { get; set; }

        protected virtual MethodInfo GetDbContextMethod { get; set; }
        protected virtual MethodInfo GetDbContextArgsMethod { get; set; }
        protected virtual MethodInfo GetDbContextAsyncMethod { get; set; }
        protected virtual MethodInfo GetDbContextAsyncArgsMethod { get; set; }

        public YoyoProDbContextProvider(IServiceProvider serviceProvider, IYoyoProDbContextTypeStorage dbContextTypeStorage, ICurrentUnitOfWorkProvider currentUnitOfWorkProvider)
        {
            this._serviceProvider = serviceProvider;
            _dbContextTypeStorage = dbContextTypeStorage;
            _currentUnitOfWorkProvider = currentUnitOfWorkProvider;
        }


        public DbContext GetDbContext()
        {
            this.CreateAbpDbContextProviderIfNotExistsOrChange();
            return (DbContext)this.GetDbContextMethod.Invoke(DbContextProvider, null);
        }

        public DbContext GetDbContext(MultiTenancySides? multiTenancySide)
        {
            this.CreateAbpDbContextProviderIfNotExistsOrChange();
            return (DbContext)this.GetDbContextArgsMethod.Invoke(DbContextProvider, new object[] { multiTenancySide });
        }

        public async Task<DbContext> GetDbContextAsync()
        {
            this.CreateAbpDbContextProviderIfNotExistsOrChange();
            var result = this.GetDbContextAsyncMethod.Invoke(DbContextProvider, null);
            await (result as Task);
            return (DbContext)result.GetType().GetProperty("Result").GetValue(result);
        }

        public async Task<DbContext> GetDbContextAsync(MultiTenancySides? multiTenancySide)
        {
            this.CreateAbpDbContextProviderIfNotExistsOrChange();
            var result = this.GetDbContextAsyncArgsMethod.Invoke(DbContextProvider, new object[] { multiTenancySide });
            await (result as Task);
            return (DbContext)result.GetType().GetProperty("Result").GetValue(result);
        }

        #region 内部方法

        protected virtual void CreateAbpDbContextProviderIfNotExistsOrChange()
        {
            var currentName = this._currentUnitOfWorkProvider.Current.GetDbContextProviderName();
            var changed = false;
            if (this.Name != currentName)
            {
                this.Name = currentName;
                changed = true;
            }

            if (this.DbContextProvider == null || changed)
            {
                this.CreateAbpDbContextProvider();
            }
        }

        protected virtual void CreateAbpDbContextProvider()
        {
            this.DbContextType = this._dbContextTypeStorage.GetDbContextType(this.Name);

            var dbContextProviderType = typeof(IDbContextProvider<>).MakeGenericType(this.DbContextType);

            DbContextProvider = this._serviceProvider.GetService(dbContextProviderType);


            var methods = dbContextProviderType.GetMethods();

            var syncMethods = methods.Where(o => o.Name == "GetDbContext");
            GetDbContextMethod = syncMethods.FirstOrDefault(o => o.GetParameters().Length == 0);
            GetDbContextArgsMethod = syncMethods.FirstOrDefault(o => o.GetParameters().Length > 0);

            var asyncMethods = methods.Where(o => o.Name == "GetDbContextAsync");
            GetDbContextAsyncMethod = asyncMethods.FirstOrDefault(o => o.GetParameters().Length == 0);
            GetDbContextAsyncArgsMethod = asyncMethods.FirstOrDefault(o => o.GetParameters().Length > 0);
        }

        #endregion

    }


}
