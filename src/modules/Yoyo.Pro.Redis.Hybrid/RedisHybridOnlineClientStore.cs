using System;
using System.Collections.Generic;
using Abp.Dependency;
using Abp.RealTime;
using Abp.Runtime.Caching.Redis;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Yoyo.Pro
{
    public class RedisHybridOnlineClientStore<T> : RedisHybridOnlineClientStore, IOnlineClientStore<T>
    {
        public RedisHybridOnlineClientStore(
            IAbpRedisCacheDatabaseProvider redisDatabaseProvider,
            IRedisCacheSerializer serializer)
            : base(redisDatabaseProvider, serializer)
        {
        }
    }

    public class RedisHybridOnlineClientStore : IOnlineClientStore, ISingletonDependency
    {
        private readonly IDatabase _database;
        private readonly IRedisCacheSerializer _serializer;

        public RedisHybridOnlineClientStore(
            IAbpRedisCacheDatabaseProvider redisDatabaseProvider,
            IRedisCacheSerializer serializer)
        {
            _database = redisDatabaseProvider.GetDatabase();
            _serializer = serializer;
        }

        public void Add(IOnlineClient client)
        {
            _database.StringSet(FormatKey(client.ConnectionId), _serializer.Serialize(client, client.GetType()));
        }

        public bool Remove(string connectionId)
        {
            return _database.KeyDelete(FormatKey(connectionId));
        }

        public bool TryRemove(string connectionId, out IOnlineClient client)
        {
            if (!TryGet(connectionId, out client))
            {
                return false;
            }

            Remove(connectionId);
            return true;
        }

        public bool TryGet(string connectionId, out IOnlineClient client)
        {
            var redisValue = _database.StringGet(FormatKey(connectionId));
            if (!redisValue.HasValue)
            {
                client = null;
                return false;
            }

            return TryDeserializeClient(redisValue, out client);
        }

        public bool Contains(string connectionId)
        {
            return _database.KeyExists(FormatKey(connectionId));
        }

        public IReadOnlyList<IOnlineClient> GetAll()
        {
            var redisResult = _database.ScriptEvaluate("return redis.call('keys', ARGV[1])", values: new RedisValue[] { FormatKey("*") });
            if (redisResult.IsNull)
            {
                return Array.Empty<IOnlineClient>();
            }

            var keys = (RedisResult[])redisResult;
            var clients = new List<IOnlineClient>(keys.Length);

            foreach (var key in keys)
            {
                var redisValue = _database.StringGet((string)key);
                if (redisValue.HasValue && TryDeserializeClient(redisValue, out var client))
                {
                    clients.Add(client);
                }
            }

            return clients;
        }

        private bool TryDeserializeClient(RedisValue redisValue, out IOnlineClient client)
        {
            try
            {
                client = (IOnlineClient)_serializer.Deserialize(redisValue);
                return client != null;
            }
            catch
            {
                try
                {
                    client = JsonConvert.DeserializeObject<OnlineClient>(redisValue);
                    return client != null;
                }
                catch
                {
                    client = null;
                    return false;
                }
            }
        }

        private static string FormatKey(string key)
        {
            return $"n:AbpOnlineClients,c:c_{key}";
        }
    }
}