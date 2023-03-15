using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Yoyo.Pro.DbStore.Switcher.Repositories;

namespace Yoyo.Pro.DbStore.Switcher
{
    public static class DbSwitcherServiceCollectionExtensions
    {
        /// <summary>
        /// 添加FreeSql支持
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDbSwitcher(this IServiceCollection services)
        {
            services.AddDbStore();
            services.TryAddTransient(typeof(IDynamicChangeRepository<,>), typeof(DynamicChangeRepository<,>));
            return services;
        }
    }
}
