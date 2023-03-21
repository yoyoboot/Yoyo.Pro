// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CSRedis;

namespace Yoyo.Pro
{
    /// <summary>
    /// CSRedis 的扩展
    /// </summary>
    public static class CSRedisClientExtensions
    {
        /// <summary>
        /// 获取 CSRedis 订阅者包装
        /// </summary>
        /// <param name="redisClient"></param>
        /// <returns></returns>
        public static ICSRedisCoreSubscriber GetSubscriber(this CSRedisClient redisClient)
        {
            return new CSRedisCoreSubscriber(redisClient);
        }

    }
}
