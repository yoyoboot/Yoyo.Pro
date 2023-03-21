// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Abp;
using CSRedis;

namespace Yoyo.Pro
{
    /// <summary>
    /// Options used to configure <see cref="RedisHubLifetimeManager{THub}"/>.
    /// </summary>
    public class RedisOptions
    {
        /// <summary>
        /// 启用YoyoPro的Redis模块, 默认为 false
        /// </summary>
        public bool UseYoyoProRedis { get; set; } = false;

        /// <summary>
        /// Gets or sets the Redis connection factory.
        /// </summary>
        public Func<Task<CSRedisClient>> ConnectionFactory { get; set; }
    }
}
