using Abp.Dependency;
using Abp.Runtime.Caching.Configuration;
using Abp.Runtime.Caching;

namespace Yoyo.Pro
{
    public class CSRedisCacheManager : CacheManagerBase<ICache>, ICacheManager
    {
        private readonly IIocManager _iocManager;

        public CSRedisCacheManager(IIocManager iocManager, ICachingConfiguration configuration) : base(configuration)
        {
            _iocManager = iocManager;
            _iocManager.RegisterIfNot<CsRedisCache>(DependencyLifeStyle.Transient);
        }

        protected override ICache CreateCacheImplementation(string name)
        {
            return _iocManager.Resolve<CsRedisCache>(new
            {
                name
            });
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
