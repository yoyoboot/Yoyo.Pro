// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSRedis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Yoyo.Pro;

namespace Yoyo.Pro
{
    public interface ISignalRCSRedisCoreConnectionProvider
    {
        Task<CSRedisClient> ConnectAsync(TextWriter log);
    }

    public class SignalRCSRedisCoreConnectionProvider : ISignalRCSRedisCoreConnectionProvider
    {
        protected readonly RedisOptions _options;
        protected readonly IServiceProvider _serviceProvider;

        public SignalRCSRedisCoreConnectionProvider(
            IOptions<RedisOptions> options,
            IServiceProvider serviceProvider
            )
        {
            _options = options.Value;
            _serviceProvider = serviceProvider;
        }



        public async Task<CSRedisClient> ConnectAsync(TextWriter log)
        {
            if (_options.UseYoyoProRedis)
            {
                var redisClientProvider = _serviceProvider
                                .GetService<ICSRedisClientProvider>();
                if (redisClientProvider == null)
                {
                    throw new Exception("请先注册 YoyoProRedisCacheModule！");
                }

                return redisClientProvider.GetDatabase();
            }


            // Factory is publicly settable. Assigning to a local variable before null check for thread safety.
            if (_options.ConnectionFactory == null)
            {
                _options.ConnectionFactory = () =>
                {
                    var client = new CSRedisClient($"localhost:6379,defaultDatabase=0");
                    return Task.FromResult(client);
                };
            }


            return await _options.ConnectionFactory?.Invoke();
        }
    }
}
