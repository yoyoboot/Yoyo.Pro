using Abp;
using Abp.Dependency;
using Abp.RealTime;
using Abp.Runtime.Caching;
using Abp.Runtime.Caching.Configuration;
using Abp.Runtime.Caching.Redis;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Yoyo.Pro
{
    public static class YoyoRedisExtensions
    {
        public static void UseYoyoProRedis(this ICachingConfiguration cachingConfiguration)
        {
            cachingConfiguration.UseYoyoProRedis(null);
        }

        public static void UseYoyoProRedis(this ICachingConfiguration cachingConfiguration,
            Action<CSRedisCacheOptions> optionsAction)
        {
            var iocManager = cachingConfiguration.AbpConfiguration.IocManager;

            iocManager.RegisterIfNot<IRedisCacheSerializer, DefaultRedisCacheSerializer>();
            iocManager.RegisterIfNot<CSRedisCacheOptions>();
            optionsAction?.Invoke(iocManager.Resolve<CSRedisCacheOptions>());

            iocManager.RegisterIfNot<ICSRedisClientProvider, CSRedisClientProvider>();
            iocManager.RegisterIfNot<ICacheManager, CSRedisCacheManager>();
            iocManager.RegisterIfNot(typeof(IOnlineClientStore<>), typeof(CSRedisOnlineClientStore<>));
        }

        public static IServiceCollection UseYoyoProRedis(this IServiceCollection services, Action<CSRedisCacheOptions> optionsAction)
        {
            Check.NotNull(optionsAction, nameof(optionsAction));

            var options = new CSRedisCacheOptions();
            optionsAction.Invoke(options);
            services.AddSingleton(options);

            services.AddSingleton<IRedisCacheSerializer, DefaultRedisCacheSerializer>();
            services.AddSingleton<ICSRedisClientProvider, CSRedisClientProvider>();
            services.AddSingleton<ICacheManager, CSRedisCacheManager>();
            services.AddSingleton<IOnlineClientStore, CSRedisOnlineClientStore>();
            services.AddSingleton(typeof(IOnlineClientStore<>), typeof(CSRedisOnlineClientStore<>));

            return services;
        }
    }
}
