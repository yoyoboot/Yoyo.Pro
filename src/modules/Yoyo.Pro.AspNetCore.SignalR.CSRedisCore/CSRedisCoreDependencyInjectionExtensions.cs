// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using CSRedis;
using Yoyo.Pro;
using Microsoft.AspNetCore.SignalR;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CSRedisCoreDependencyInjectionExtensions
    {
        /// <summary>
        /// Adds scale-out to a <see cref="ISignalRServerBuilder"/>, using a shared Redis server.
        /// </summary>
        /// <param name="signalrBuilder">The <see cref="ISignalRServerBuilder"/>.</param>
        /// <returns>The same instance of the <see cref="ISignalRServerBuilder"/> for chaining.</returns>
        public static ISignalRServerBuilder AddCSRedisCore(this ISignalRServerBuilder signalrBuilder)
        {
            return AddCSRedisCore(signalrBuilder, o => { });
        }

        /// <summary>
        /// Adds scale-out to a <see cref="ISignalRServerBuilder"/>, using a shared Redis server.
        /// </summary>
        /// <param name="signalrBuilder">The <see cref="ISignalRServerBuilder"/>.</param>
        /// <param name="connectionFactory">Gets or sets the Redis connection factory.</param>
        /// <returns>The same instance of the <see cref="ISignalRServerBuilder"/> for chaining.</returns>
        public static ISignalRServerBuilder AddCSRedisCore(this ISignalRServerBuilder signalrBuilder, Func<Task<CSRedisClient>> connectionFactory)
        {
            return AddCSRedisCore(signalrBuilder, o =>
            {
                o.ConnectionFactory = connectionFactory;
            });
        }

        /// <summary>
        /// Adds scale-out to a <see cref="ISignalRServerBuilder"/>, using a shared Redis server.
        /// </summary>
        /// <param name="signalrBuilder">The <see cref="ISignalRServerBuilder"/>.</param>
        /// <param name="configure">A callback to configure the Redis options.</param>
        /// <returns>The same instance of the <see cref="ISignalRServerBuilder"/> for chaining.</returns>
        public static ISignalRServerBuilder AddCSRedisCore(this ISignalRServerBuilder signalrBuilder, Action<RedisOptions> configure)
        {
            signalrBuilder.Services.Configure(configure);
            signalrBuilder.Services.AddSingleton(typeof(HubLifetimeManager<>), typeof(RedisHubLifetimeManager<>));
            signalrBuilder.Services.AddSingleton(typeof(ISignalRCSRedisCoreConnectionProvider), typeof(SignalRCSRedisCoreConnectionProvider));
            return signalrBuilder;
        }

        /// <summary>
        /// Adds scale-out to a <see cref="ISignalRServerBuilder"/>, using a shared Redis server.
        /// </summary>
        /// <param name="signalrBuilder">The <see cref="ISignalRServerBuilder"/>.</param>
        /// <param name="connectionFactory">Gets or sets the Redis connection factory.</param>
        /// <param name="configure">A callback to configure the Redis options.</param>
        /// <returns>The same instance of the <see cref="ISignalRServerBuilder"/> for chaining.</returns>
        public static ISignalRServerBuilder AddCSRedisCore(this ISignalRServerBuilder signalrBuilder, Func<Task<CSRedisClient>> connectionFactory, Action<RedisOptions> configure)
        {
            return AddCSRedisCore(signalrBuilder, o =>
            {
                o.ConnectionFactory = connectionFactory;
                configure(o);
            });
        }
    }
}
