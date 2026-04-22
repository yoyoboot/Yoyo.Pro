// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Text;
using CSRedis;

namespace Yoyo.Pro
{
    /// <summary>
    ///  CSRedis 订阅者的数据
    /// </summary>
    public class CSRedisCoreSubscribeMessageEventArgs : CSRedisClient.SubscribeMessageEventArgs
    {

        public byte[] Message { get; }


        public CSRedisCoreSubscribeMessageEventArgs(CSRedisClient.SubscribeMessageEventArgs args)
        {
            this.MessageId = args.MessageId;
            this.Channel = args.Channel;
            this.Body = args.Body;

            if (!string.IsNullOrWhiteSpace(this.Body))
            {
                this.Message = Convert.FromBase64String(this.Body);
            }
        }

    }
}
