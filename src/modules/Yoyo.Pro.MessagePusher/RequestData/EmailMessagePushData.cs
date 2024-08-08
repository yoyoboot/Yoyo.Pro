// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Yoyo.Pro.RequestData
{
    /// <summary>
    /// 邮件通知Dto
    /// </summary>
    public class EmailMessagePushData : MessagePushDataBase
    {
        /// <summary>
        /// 邮箱服务器
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// 启用ssl
        /// </summary>
        public bool UseSsl { get; set; }
        /// <summary>
        /// 发送者名称
        /// </summary>
        public string FromName { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 主题
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 收件人邮箱列表
        /// </summary>
        public List<string> EmailList { get; set; } = new List<string>();
    }
}
