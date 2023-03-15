using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Runtime.Caching;
using Abp.Domain.Entities;
using System.Reflection;
using Abp.Reflection.Extensions;
using Abp;
using System.Linq;
using Abp.Extensions;
using Abp.Runtime.Caching.Redis;
using Abp.Threading;
using CSRedis;

// ReSharper disable CognitiveComplexity

namespace Yoyo.Pro
{
    public class CsRedisCache : CacheBase
    {
        private readonly CSRedisClient _database;
        private readonly IRedisCacheSerializer _serializer;

        public CsRedisCache(string name,
            ICSRedisClientProvider redisProvider,
            IRedisCacheSerializer serializer) : base(name)
        {
            _serializer = serializer;
            _database = redisProvider.GetDatabase();
        }

        #region > 查询相关 <

        public override object GetOrDefault(string key)
        {
            var redisValue = _database.Get(GetLocalizedRedisKey(key));
            return !redisValue.IsNullOrEmpty() ? Deserialize(redisValue) : null;
        }

        public override object[] GetOrDefault(string[] keys)
        {
            var redisKeys = keys.Select(GetLocalizedRedisKey);
            var objBytes = Get(redisKeys);
            return objBytes.ToArray();
        }

        public override async Task<object> GetOrDefaultAsync(string key)
        {
            var redisKey = GetLocalizedRedisKey(key);
            var redisValue = await _database.GetAsync(redisKey);

            return string.IsNullOrWhiteSpace(redisValue) ? null : Deserialize(redisValue);
        }

        public override async Task<object[]> GetOrDefaultAsync(string[] keys)
        {
            var redisValues = new List<object>();
            foreach (var key in keys)
            {
                redisValues.Add(await GetOrDefaultAsync(key));
            }

            return redisValues.ToArray();
        }

        public override bool TryGetValue(string key, out object value)
        {
            var cacheValue = GetOrDefault(key);

            value = cacheValue;

            return cacheValue != null;
        }

        #endregion

        #region > 缓存设置相关 <

        public override async Task SetAsync(string key,
            object value,
            TimeSpan? slidingExpireTime = null,
            DateTimeOffset? absoluteExpireTime = null)
        {
            if (value == null)
            {
                throw new AbpException("不能将 NULL 插入缓存。");
            }

            var redisKey = GetLocalizedRedisKey(key);
            var redisValue = Serialize(value, GetSerializableType(value));

            if (absoluteExpireTime.HasValue)
            {
                if (!await _database.SetAsync(redisKey, redisValue))
                {
                    Logger.ErrorFormat("不能在 Redis 设置 Key 为 {0}，值为 {1} 的数据。", redisKey, redisValue);
                }
                else if (!await _database.ExpireAtAsync(redisKey, absoluteExpireTime.Value.Date))
                {
                    Logger.ErrorFormat("不能在 Redis 设置 Key 为 {0}，值为 {1:O} 的绝对过期时间。", redisKey, redisValue);
                }
            }
            else if (slidingExpireTime.HasValue)
            {
                if (!await _database.SetAsync(redisKey, redisValue, slidingExpireTime.Value))
                {
                    Logger.ErrorFormat("不能在 Redis 设置 Key 为 {0} 值为: {1}，滑动过期时间为 {2:c}的数据。", redisKey, redisValue, slidingExpireTime.Value);
                }
            }
            else if (DefaultAbsoluteExpireTime.HasValue)
            {
                if (!await _database.SetAsync(redisKey, redisValue))
                {
                    Logger.ErrorFormat("不能在 Redis 设置 Key 为 {0}，值为 {1} 的数据。", redisKey, redisValue);
                }
                else if (!await _database.ExpireAtAsync(redisKey, DefaultAbsoluteExpireTime.Value.Date))
                {
                    Logger.ErrorFormat("不能在 Redis 设置 Key 为 {0}，值为 {1:O} 的绝对过期时间。", redisKey, redisValue);
                }
            }
            else
            {
                if (!await _database.SetAsync(redisKey, redisValue, DefaultSlidingExpireTime))
                {
                    Logger.ErrorFormat("不能在 Redis 设置 Key 为 {0} 值为: {1}，滑动过期时间为 {2:c}的数据。", redisKey, redisValue, DefaultSlidingExpireTime);
                }
            }
        }

        public override void Set(string key,
            object value,
            TimeSpan? slidingExpireTime = null,
            DateTimeOffset? absoluteExpireTime = null)
        {
            AsyncHelper.RunSync(() => SetAsync(key, value, slidingExpireTime, absoluteExpireTime));
        }

        #endregion

        #region > 删除相关 <

        public override void Remove(string key) => _database.Del(GetLocalizedRedisKey(key));

        public override Task RemoveAsync(string key) => _database.DelAsync(GetLocalizedRedisKey(key));

        public override void Clear() => _database.Del(_database.Keys(GetLocalizedRedisKey("*")));

        public override void Remove(string[] keys) => _database.Del(keys.Select(GetLocalizedRedisKey).ToArray());

        public override Task RemoveAsync(string[] keys) => _database.DelAsync(keys.Select(GetLocalizedRedisKey).ToArray());

        #endregion

        #region > 内部函数 <

        protected IEnumerable<object> Get(IEnumerable<string> keys)
        {
            foreach (var key in keys)
            {
                yield return GetOrDefault(key);
            }
        }

        protected virtual Type GetSerializableType(object value)
        {
            var type = value.GetType();

            // ReSharper disable once PossibleNullReferenceException
            if (EntityHelper.IsEntity(type) &&
                type.GetAssembly().FullName.Contains("EntityFrameworkDynamicProxies"))
            {
                type = type.GetTypeInfo().BaseType;
            }

            return type;
        }

        protected virtual string Serialize(object value, Type type) => _serializer.Serialize(value, type);

        protected virtual object Deserialize(string objectByte) => _serializer.Deserialize(objectByte);

        protected virtual string GetLocalizedRedisKey(string key) => GetLocalizedKey(key);

        protected virtual string GetLocalizedKey(string key) => $"n:{Name},c:{key}";

        #endregion
    }
}
