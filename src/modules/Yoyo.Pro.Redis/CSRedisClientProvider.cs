using CSRedis;
using System;

namespace Yoyo.Pro
{
    public class CSRedisClientProvider : ICSRedisClientProvider
    {
        private readonly CSRedisCacheOptions _options;
        private readonly Lazy<CSRedisClient> _client;

        public CSRedisClientProvider(CSRedisCacheOptions options)
        {
            _options = options;
            _client = new Lazy<CSRedisClient>(CreateRedisClient);
        }

        public CSRedisClient GetDatabase() => _client.Value;

        protected CSRedisClient CreateRedisClient()
        {
            return new CSRedisClient(_options.ConnectionString.EndsWith(",")
                ? $"{_options.ConnectionString}defaultDatabase={_options.DatabaseId}"
                : $"{_options.ConnectionString},defaultDatabase={_options.DatabaseId}");
        }
    }
}
