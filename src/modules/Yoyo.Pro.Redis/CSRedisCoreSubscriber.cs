// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSRedis;

namespace Yoyo.Pro
{
    /// <summary>
    /// CSRedis 订阅者包装的默认实现
    /// </summary>
    public class CSRedisCoreSubscriber : ICSRedisCoreSubscriber
    {
        public virtual CSRedisClient Client { get; }

        protected virtual ConcurrentDictionary<string, CSRedisClient.SubscribeObject> SubscribeObjectMap { get; }

        public CSRedisCoreSubscriber(CSRedisClient client)
        {
            Client = client;

            SubscribeObjectMap = new ConcurrentDictionary<string, CSRedisClient.SubscribeObject>();
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="channel">频道</param>
        /// <param name="payload">数据</param>
        /// <returns></returns>
        public virtual async Task PublishAsync(string channel, byte[] payload)
        {
            var str = Convert.ToBase64String(payload);

            await this.Client.PublishAsync(channel, str);
        }

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="channel">频道</param>
        /// <param name="onMessage">消息事件</param>
        /// <returns></returns>
        public virtual async Task SubscribeAsync(
            string channel,
            Action<CSRedisCoreSubscribeMessageEventArgs> onMessage
            )
        {
            await Task.Yield();

            // 构建
            var subscribe = (channel, new Action<CSRedisClient.SubscribeMessageEventArgs>((args) =>
            {
                onMessage?.Invoke(
                    new CSRedisCoreSubscribeMessageEventArgs(args)
                    );
            }));

            // 创建订阅
            var subscribeObject = this.Client.Subscribe(subscribe);

            // 存储
            SubscribeObjectMap.AddOrUpdate(channel, subscribeObject, (k, v) =>
            {
                if (!v.IsUnsubscribed)
                {
                    v.Unsubscribe();
                }
                v.Dispose();

                return subscribeObject;
            });
        }

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="channel">频道</param>
        /// <returns></returns>
        public virtual async Task UnsubscribeAsync(string channel)
        {
            await Task.Yield();

            if (!SubscribeObjectMap.TryRemove(channel, out var result))
            {
                return;
            }

            if (!result.IsUnsubscribed)
            {
                result.Unsubscribe();
            }
            result.Dispose();
        }

        /// <summary>
        /// 取消所有订阅
        /// </summary>
        public virtual void UnsubscribeAll()
        {
            foreach (var item in SubscribeObjectMap)
            {
                if (!item.Value.IsUnsubscribed)
                {
                    item.Value.Unsubscribe();
                }
                item.Value.Dispose();
            }
            SubscribeObjectMap.Clear();
        }

        public void Dispose()
        {
            this.UnsubscribeAll();
        }
    }
}
