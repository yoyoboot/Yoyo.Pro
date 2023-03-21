// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;

namespace Yoyo.Pro
{
    /// <summary>
    /// CSRedis 订阅者包装
    /// </summary>
    public interface ICSRedisCoreSubscriber : IDisposable
    {
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="channel">频道</param>
        /// <param name="payload">数据</param>
        /// <returns></returns>
        Task PublishAsync(string channel, byte[] payload);


        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="channel">频道</param>
        /// <param name="onMessage">消息事件</param>
        /// <returns></returns>
        Task SubscribeAsync(string channel, Action<CSRedisCoreSubscribeMessageEventArgs> onMessage);


        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="channel">频道</param>
        /// <returns></returns>
        Task UnsubscribeAsync(string channel);


        /// <summary>
        /// 取消所有订阅
        /// </summary>
        void UnsubscribeAll();
    }
}
