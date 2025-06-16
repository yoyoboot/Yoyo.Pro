using System;
using Abp;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class LazyServiceProvderExtensions
    {
        public static Lazy<T> GetLazy<T>(this IServiceProvider serviceProvider)
             where T : class
        {
            Check.NotNull(serviceProvider, nameof(serviceProvider));

            return new Lazy<T>(() =>
            {
                return serviceProvider.GetRequiredService<T>();
            });
        }

        public static Lazy<T> GetLazy<T>(this ILazyServiceProvider lazyServiceProvider)
             where T : class
        {
            Check.NotNull(lazyServiceProvider, nameof(lazyServiceProvider));

            return new Lazy<T>(() =>
            {
                return lazyServiceProvider.LazyGetRequiredService<T>();
            });
        }

        public static IRepository<T> GetRepository<T>(this IServiceProvider serviceProvider)
        where T : Entity<string>
        {
            Check.NotNull(serviceProvider, nameof(serviceProvider));

            return serviceProvider.GetRequiredService<IRepository<T>>();
        }

        public static Lazy<IRepository<TEntity, TPrimaryKey>> GetLazyRepository<TEntity, TPrimaryKey>(this IServiceProvider serviceProvider)
            where TEntity : Entity<TPrimaryKey>
        {
            Check.NotNull(serviceProvider, nameof(serviceProvider));

            return serviceProvider.GetLazy<IRepository<TEntity, TPrimaryKey>>();
        }

        public static Lazy<IRepository<TEntity, string>> GetLazyRepository<TEntity>(this IServiceProvider serviceProvider)
            where TEntity : Entity<string>
        {
            Check.NotNull(serviceProvider, nameof(serviceProvider));

            return serviceProvider.GetLazyRepository<TEntity, string>();
        }
    }
}
