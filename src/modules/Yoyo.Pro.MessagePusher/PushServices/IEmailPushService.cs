// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Castle.Core.Logging;
using MailKit.Net.Smtp;
using MimeKit;
using Yoyo.Pro.RequestData;

namespace Yoyo.Pro.PushServices
{
    /// <summary>
    /// 邮件通知接口
    /// </summary>
    public interface IEmailPushService
    {
        Task<string> SendMessage(EmailMessagePushData data);
    }
    /// <summary>
    /// 邮件通知服务
    /// </summary>
    public class EmailPushService : IEmailPushService
    {
        protected readonly ILogger _logger;

        public EmailPushService(ILogger logger = null)
        {
            _logger = logger;
        }

        /// <summary>
        /// 邮件通知服务
        /// </summary>
        /// <param name="data">邮件通知内容</param>
        public virtual async Task<string> SendMessage(EmailMessagePushData data)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(data.FromName, data.UserName));
            message.To.Add(new MailboxAddress(data.ToName, data.ToEmailAddress));
            message.Subject = data.Subject;

            message.Body = new TextPart("plain")
            {
                Text = data.Content
            };

            try
            {
                using (var client = new SmtpClient())
                {
                    client.Connect(data.Host, data.Port, false);

                    // Note: only needed if the SMTP server requires authentication
                    client.Authenticate(data.UserName, data.Password);

                    var result = await client.SendAsync(message);
                    client.Disconnect(true);
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger?.Error(ex.Message);
                throw;
            }
        }
    }
}
