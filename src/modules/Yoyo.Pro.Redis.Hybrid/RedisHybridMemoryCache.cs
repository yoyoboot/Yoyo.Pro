using Microsoft.Extensions.Caching.Memory;

namespace Yoyo.Pro
{
    public interface IRedisHybridMemoryCache : IMemoryCache
    {
    }

    public sealed class RedisHybridMemoryCache : MemoryCache, IRedisHybridMemoryCache
    {
        public RedisHybridMemoryCache()
            : base(new MemoryCacheOptions())
        {
        }
    }
}