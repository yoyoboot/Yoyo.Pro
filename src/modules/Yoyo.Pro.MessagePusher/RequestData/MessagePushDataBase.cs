// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Yoyo.Pro.RequestData
{
    /// <summary>
    /// 短信通知基础Dto
    /// </summary>
    public class MessagePushDataBase
    {
        /// <summary>
        /// 消息唯一标识
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 调用者唯一标识，标识可重复
        /// </summary>
        public Guid Caller { get; set; }
        /// <summary>
        /// 推送渠道
        /// </summary>
        public PushChannelType PushChannelType { get; set; }
        /// <summary>
        /// 租户id
        /// </summary>
        public string TenantId { get; set; }
    }
}
