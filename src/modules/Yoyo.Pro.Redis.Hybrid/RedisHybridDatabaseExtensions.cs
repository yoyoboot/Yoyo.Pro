using System;
using StackExchange.Redis;

namespace Yoyo.Pro
{
    internal static class RedisHybridDatabaseExtensions
    {
        public static void KeyDeleteWithPrefix(this IDatabase database, string prefix)
        {
            if (database == null)
            {
                throw new ArgumentException("Database cannot be null", nameof(database));
            }

            if (string.IsNullOrWhiteSpace(prefix))
            {
                throw new ArgumentException("Prefix cannot be empty", nameof(prefix));
            }

            database.ScriptEvaluate(@"
                local keys = redis.call('keys', ARGV[1])
                for i=1,#keys,5000 do
                redis.call('del', unpack(keys, i, math.min(i+4999, #keys)))
                end", values: new RedisValue[] { prefix });
        }
    }
}
