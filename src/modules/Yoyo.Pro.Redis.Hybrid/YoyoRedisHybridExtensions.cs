using System;
using Abp.Dependency;
using Abp.RealTime;
using Abp.Runtime.Caching;
using Abp.Runtime.Caching.Configuration;
using Abp.Runtime.Caching.Redis;
using Castle.MicroKernel.Registration;

namespace Yoyo.Pro
{
    public static class YoyoRedisHybridExtensions
    {
        public static void UseYoyoProRedisHybrid(this ICachingConfiguration cachingConfiguration)
        {
            cachingConfiguration.UseYoyoProRedisHybrid(_ => { });
        }

        public static void UseYoyoProRedisHybrid(
            this ICachingConfiguration cachingConfiguration,
            Action<AbpRedisCacheOptions> optionsAction)
        {
            var iocManager = cachingConfiguration.AbpConfiguration.IocManager;

            optionsAction?.Invoke(iocManager.Resolve<AbpRedisCacheOptions>());

            if (!iocManager.IsRegistered<IRedisHybridMemoryCache>())
            {
                iocManager.IocContainer.Register(
                    Component.For<IRedisHybridMemoryCache>()
                        .ImplementedBy<RedisHybridMemoryCache>()
                        .LifestyleSingleton()
                        .IsDefault());
            }

            iocManager.RegisterIfNot<ICacheManager, RedisHybridCacheManager>();
            iocManager.RegisterIfNot<IOnlineClientStore, RedisHybridOnlineClientStore>();
            iocManager.RegisterIfNot(typeof(IOnlineClientStore<>), typeof(RedisHybridOnlineClientStore<>));
        }
    }
}
