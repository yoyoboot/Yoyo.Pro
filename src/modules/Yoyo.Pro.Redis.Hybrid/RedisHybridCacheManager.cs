using Abp.Dependency;
using Abp.Runtime.Caching;
using Abp.Runtime.Caching.Configuration;

namespace Yoyo.Pro
{
    public class RedisHybridCacheManager : CacheManagerBase<ICache>, ICacheManager
    {
        private readonly IIocManager _iocManager;

        public RedisHybridCacheManager(IIocManager iocManager, ICachingConfiguration configuration)
            : base(configuration)
        {
            _iocManager = iocManager;
            _iocManager.RegisterIfNot<RedisHybridCache>(DependencyLifeStyle.Transient);
        }

        protected override ICache CreateCacheImplementation(string name)
        {
            return _iocManager.Resolve<RedisHybridCache>(new { name });
        }

        protected override void DisposeCaches()
        {
            foreach (var cache in Caches)
            {
                _iocManager.Release(cache.Value);
            }
        }
    }
}
