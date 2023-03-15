using CSRedis;

namespace Yoyo.Pro
{
    public interface ICSRedisClientProvider
    {
        CSRedisClient GetDatabase();
    }
}
