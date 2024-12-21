using System.Collections.Generic;
using Abp.Dependency;
using Abp.RealTime;

namespace Yoyo.Pro
{
    public class CSRedisOnlineClientStore<T> : CSRedisOnlineClientStore, IOnlineClientStore<T>
    {
        public CSRedisOnlineClientStore(
            ICSRedisClientProvider redisClientProvider) : base(redisClientProvider)
        {
        }
    }

    public class CSRedisOnlineClientStore : IOnlineClientStore, ISingletonDependency
    {

        readonly ICSRedisClientProvider _redisClientProvider;

        private readonly string _clientStoreKey;
        private readonly string _userStoreKey;

        public CSRedisOnlineClientStore(ICSRedisClientProvider redisClientProvider)
        {
            _redisClientProvider = redisClientProvider;
        }

        public void Add(IOnlineClient client)
        {
            _redisClientProvider.GetDatabase().Set(FormatKey(client.ConnectionId), client);
        }

        public bool Remove(string connectionId)
        {
            _redisClientProvider.GetDatabase().Del(FormatKey(connectionId));

            return true;
        }

        public bool TryRemove(string connectionId, out IOnlineClient client)
        {
            client = null;
            if (this.Contains(connectionId))
            {
                client = _redisClientProvider.GetDatabase().Get<OnlineClient>(FormatKey(connectionId));
                this.Remove(connectionId);
                return true;
            }

            return false;
        }

        public bool TryGet(string connectionId, out IOnlineClient client)
        {
            client = null;
            if (this.Contains(connectionId))
            {
                client = _redisClientProvider.GetDatabase().Get<OnlineClient>(FormatKey(connectionId));
                return true;
            }

            return false;
        }

        public bool Contains(string connectionId)
        {
            return _redisClientProvider.GetDatabase().Exists(FormatKey(connectionId));
        }

        public IReadOnlyList<IOnlineClient> GetAll()
        {
            var resList = new List<IOnlineClient>();

            var keys = _redisClientProvider.GetDatabase().Keys(FormatKey("*"));
            foreach (var key in keys)
            {
                resList.Add(_redisClientProvider.GetDatabase().Get<OnlineClient>(key));
            }

            return resList;
        }


        static string FormatKey(string key)
        {
            return $"n:AbpOnlineClients,c:c_{key}";
        }

    }
}
