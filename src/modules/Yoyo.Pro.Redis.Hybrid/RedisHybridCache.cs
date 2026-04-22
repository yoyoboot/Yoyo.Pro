using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Entities;
using Abp.Reflection.Extensions;
using Abp.Runtime.Caching;
using Abp.Runtime.Caching.Redis;
using Microsoft.Extensions.Caching.Memory;
using StackExchange.Redis;

namespace Yoyo.Pro
{
    public class RedisHybridCache : CacheBase
    {
        /// <summary>
        /// 默认30秒
        /// </summary>
        public static TimeSpan Level1CacheExpiration { get; set; } = TimeSpan.FromSeconds(30);

        private readonly IDatabase _database;
        private readonly IRedisHybridMemoryCache _memoryCache;
        private readonly IRedisCacheSerializer _serializer;
        private readonly ConcurrentDictionary<string, byte> _level1Keys;

        public RedisHybridCache(
            string name,
            IAbpRedisCacheDatabaseProvider redisDatabaseProvider,
            IRedisHybridMemoryCache memoryCache,
            IRedisCacheSerializer serializer)
            : base(name)
        {
            _database = redisDatabaseProvider.GetDatabase();
            _memoryCache = memoryCache;
            _serializer = serializer;
            _level1Keys = new ConcurrentDictionary<string, byte>();
        }

        public override object GetOrDefault(string key)
        {
            if (TryGetFromLevel1Cache(key, out var value))
            {
                return value;
            }

            var redisValue = _database.StringGet(GetLocalizedRedisKey(key));
            if (!redisValue.HasValue)
            {
                return null;
            }

            value = Deserialize(redisValue);
            SetLevel1Cache(key, value);
            return value;
        }

        public override object[] GetOrDefault(string[] keys)
        {
            var values = new object[keys.Length];
            var missedIndexes = new ConcurrentBag<int>();

            for (var index = 0; index < keys.Length; index++)
            {
                if (TryGetFromLevel1Cache(keys[index], out var value))
                {
                    values[index] = value;
                }
                else
                {
                    missedIndexes.Add(index);
                }
            }

            if (missedIndexes.IsEmpty)
            {
                return values;
            }

            var orderedMisses = missedIndexes.OrderBy(index => index).ToArray();
            var redisKeys = orderedMisses.Select(index => GetLocalizedRedisKey(keys[index])).ToArray();
            var redisValues = _database.StringGet(redisKeys);

            for (var index = 0; index < orderedMisses.Length; index++)
            {
                var sourceIndex = orderedMisses[index];
                var redisValue = redisValues[index];
                if (!redisValue.HasValue)
                {
                    continue;
                }

                var value = Deserialize(redisValue);
                values[sourceIndex] = value;
                SetLevel1Cache(keys[sourceIndex], value);
            }

            return values;
        }

        public override async Task<object> GetOrDefaultAsync(string key)
        {
            var redisValue = await _database.StringGetAsync(GetLocalizedRedisKey(key));
            if (!redisValue.HasValue)
            {
                return null;
            }

            return Deserialize(redisValue);
        }

        public override async Task<object[]> GetOrDefaultAsync(string[] keys)
        {
            var redisKeys = keys.Select(GetLocalizedRedisKey).ToArray();
            var redisValues = await _database.StringGetAsync(redisKeys);
            return redisValues.Select(redisValue => redisValue.HasValue ? Deserialize(redisValue) : null).ToArray();
        }

        public override bool TryGetValue(string key, out object value)
        {
            if (TryGetFromLevel1Cache(key, out value))
            {
                return true;
            }

            var redisValue = _database.StringGet(GetLocalizedRedisKey(key));
            if (!redisValue.HasValue)
            {
                value = null;
                return false;
            }

            value = Deserialize(redisValue);
            SetLevel1Cache(key, value);
            return true;
        }

        public override void Set(
            string key,
            object value,
            TimeSpan? slidingExpireTime = null,
            DateTimeOffset? absoluteExpireTime = null)
        {
            if (value == null)
            {
                throw new AbpException("Can not insert null values to the cache!");
            }

            var redisKey = GetLocalizedRedisKey(key);
            var redisValue = Serialize(value, GetSerializableType(value));
            var writeSucceeded = false;

            if (absoluteExpireTime.HasValue)
            {
                if (!_database.StringSet(redisKey, redisValue))
                {
                    Logger.ErrorFormat("Unable to set key:{0} value:{1} in Redis", redisKey, redisValue);
                }
                else if (!_database.KeyExpire(redisKey, absoluteExpireTime.Value.UtcDateTime))
                {
                    Logger.ErrorFormat("Unable to set key:{0} to expire at {1:O} in Redis", redisKey, absoluteExpireTime.Value.UtcDateTime);
                }
                else
                {
                    writeSucceeded = true;
                }
            }
            else if (slidingExpireTime.HasValue)
            {
                if (!_database.StringSet(redisKey, redisValue, slidingExpireTime.Value))
                {
                    Logger.ErrorFormat("Unable to set key:{0} value:{1} to expire after {2:c} in Redis", redisKey, redisValue, slidingExpireTime.Value);
                }
                else
                {
                    writeSucceeded = true;
                }
            }
            else if (DefaultAbsoluteExpireTime.HasValue)
            {
                if (!_database.StringSet(redisKey, redisValue))
                {
                    Logger.ErrorFormat("Unable to set key:{0} value:{1} in Redis", redisKey, redisValue);
                }
                else if (!_database.KeyExpire(redisKey, DefaultAbsoluteExpireTime.Value.UtcDateTime))
                {
                    Logger.ErrorFormat("Unable to set key:{0} to expire at {1:O} in Redis", redisKey, DefaultAbsoluteExpireTime.Value.UtcDateTime);
                }
                else
                {
                    writeSucceeded = true;
                }
            }
            else
            {
                if (!_database.StringSet(redisKey, redisValue, DefaultSlidingExpireTime))
                {
                    Logger.ErrorFormat("Unable to set key:{0} value:{1} to expire after {2:c} in Redis", redisKey, redisValue, DefaultSlidingExpireTime);
                }
                else
                {
                    writeSucceeded = true;
                }
            }

            if (writeSucceeded)
            {
                SetLevel1Cache(key, value, slidingExpireTime, absoluteExpireTime);
            }
        }

        public override async Task SetAsync(
            string key,
            object value,
            TimeSpan? slidingExpireTime = null,
            DateTimeOffset? absoluteExpireTime = null)
        {
            if (value == null)
            {
                throw new AbpException("Can not insert null values to the cache!");
            }

            var redisKey = GetLocalizedRedisKey(key);
            var redisValue = Serialize(value, GetSerializableType(value));

            if (absoluteExpireTime.HasValue)
            {
                if (!await _database.StringSetAsync(redisKey, redisValue))
                {
                    Logger.ErrorFormat("Unable to set key:{0} value:{1} asynchronously in Redis", redisKey, redisValue);
                }
                else if (!await _database.KeyExpireAsync(redisKey, absoluteExpireTime.Value.UtcDateTime))
                {
                    Logger.ErrorFormat("Unable to set key:{0} to expire at {1:O} asynchronously in Redis", redisKey, absoluteExpireTime.Value.UtcDateTime);
                }
            }
            else if (slidingExpireTime.HasValue)
            {
                if (!await _database.StringSetAsync(redisKey, redisValue, slidingExpireTime.Value))
                {
                    Logger.ErrorFormat("Unable to set key:{0} value:{1} to expire after {2:c} asynchronously in Redis", redisKey, redisValue, slidingExpireTime.Value);
                }
            }
            else if (DefaultAbsoluteExpireTime.HasValue)
            {
                if (!await _database.StringSetAsync(redisKey, redisValue))
                {
                    Logger.ErrorFormat("Unable to set key:{0} value:{1} asynchronously in Redis", redisKey, redisValue);
                }
                else if (!await _database.KeyExpireAsync(redisKey, DefaultAbsoluteExpireTime.Value.UtcDateTime))
                {
                    Logger.ErrorFormat("Unable to set key:{0} to expire at {1:O} asynchronously in Redis", redisKey, DefaultAbsoluteExpireTime.Value.UtcDateTime);
                }
            }
            else
            {
                if (!await _database.StringSetAsync(redisKey, redisValue, DefaultSlidingExpireTime))
                {
                    Logger.ErrorFormat("Unable to set key:{0} value:{1} to expire after {2:c} asynchronously in Redis", redisKey, redisValue, DefaultSlidingExpireTime);
                }
            }
        }

        public override void Remove(string key)
        {
            RemoveLevel1Cache(key);
            _database.KeyDelete(GetLocalizedRedisKey(key));
        }

        public override Task RemoveAsync(string key)
        {
            return _database.KeyDeleteAsync(GetLocalizedRedisKey(key));
        }

        public override void Remove(string[] keys)
        {
            foreach (var key in keys)
            {
                RemoveLevel1Cache(key);
            }

            var redisKeys = keys.Select(GetLocalizedRedisKey).ToArray();
            _database.KeyDelete(redisKeys);
        }

        public override Task RemoveAsync(string[] keys)
        {
            var redisKeys = keys.Select(GetLocalizedRedisKey).ToArray();
            return _database.KeyDeleteAsync(redisKeys);
        }

        public override void Clear()
        {
            foreach (var memoryKey in _level1Keys.Keys)
            {
                _memoryCache.Remove(memoryKey);
            }

            _database.KeyDeleteWithPrefix(GetLocalizedRedisKey("*"));
        }

        protected virtual Type GetSerializableType(object value)
        {
            var type = value.GetType();
            if (EntityHelper.IsEntity(type) && type.GetAssembly().FullName.Contains("EntityFrameworkDynamicProxies"))
            {
                type = type.GetTypeInfo().BaseType;
            }

            return type;
        }

        protected virtual RedisValue Serialize(object value, Type type)
        {
            return _serializer.Serialize(value, type);
        }

        protected virtual object Deserialize(RedisValue objectByte)
        {
            return _serializer.Deserialize(objectByte);
        }

        protected virtual RedisKey GetLocalizedRedisKey(string key)
        {
            return GetLocalizedKey(key);
        }

        protected virtual string GetLocalizedKey(string key)
        {
            return $"n:{Name},c:{key}";
        }

        private bool TryGetFromLevel1Cache(string key, out object value)
        {
            return _memoryCache.TryGetValue(GetLevel1CacheKey(key), out value);
        }

        private void SetLevel1Cache(
            string key,
            object value,
            TimeSpan? slidingExpireTime = null,
            DateTimeOffset? absoluteExpireTime = null)
        {
            var memoryKey = GetLevel1CacheKey(key);
            _level1Keys[memoryKey] = 0;
            _memoryCache.Set(memoryKey, value, CreateLevel1CacheOptions(memoryKey, slidingExpireTime, absoluteExpireTime));
        }

        private void RemoveLevel1Cache(string key)
        {
            var memoryKey = GetLevel1CacheKey(key);
            _memoryCache.Remove(memoryKey);
            _level1Keys.TryRemove(memoryKey, out _);
        }

        private string GetLevel1CacheKey(string key)
        {
            return GetLocalizedKey(key);
        }

        private MemoryCacheEntryOptions CreateLevel1CacheOptions(
            string memoryKey,
            TimeSpan? slidingExpireTime,
            DateTimeOffset? absoluteExpireTime)
        {
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = GetLevel1AbsoluteExpiration(slidingExpireTime, absoluteExpireTime)
            };

            options.RegisterPostEvictionCallback((cacheKey, _, _, _) =>
            {
                if (cacheKey is string evictedKey)
                {
                    _level1Keys.TryRemove(evictedKey, out _);
                }
            });

            return options;
        }

        private DateTimeOffset GetLevel1AbsoluteExpiration(
            TimeSpan? slidingExpireTime,
            DateTimeOffset? absoluteExpireTime)
        {
            var now = DateTimeOffset.UtcNow;
            var level1AbsoluteExpiration = now.Add(Level1CacheExpiration);

            if (absoluteExpireTime.HasValue && absoluteExpireTime.Value < level1AbsoluteExpiration)
            {
                return absoluteExpireTime.Value;
            }

            var effectiveSlidingExpireTime = slidingExpireTime;
            if (!effectiveSlidingExpireTime.HasValue && DefaultAbsoluteExpireTime.HasValue)
            {
                return DefaultAbsoluteExpireTime.Value < level1AbsoluteExpiration
                    ? DefaultAbsoluteExpireTime.Value
                    : level1AbsoluteExpiration;
            }

            if (!effectiveSlidingExpireTime.HasValue)
            {
                effectiveSlidingExpireTime = DefaultSlidingExpireTime;
            }

            if (effectiveSlidingExpireTime.HasValue)
            {
                var candidateExpiration = now.Add(effectiveSlidingExpireTime.Value);
                if (candidateExpiration < level1AbsoluteExpiration)
                {
                    return candidateExpiration;
                }
            }

            return level1AbsoluteExpiration;
        }
    }
}
