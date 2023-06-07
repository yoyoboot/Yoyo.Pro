// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Yoyo.Pro.RequestData;
using MailKit.Net.Smtp;
using MimeKit;

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
        /// <summary>
        /// 邮件通知服务
        /// </summary>
        /// <param name="data">邮件通知内容</param>
        public async Task<string> SendMessage(EmailMessagePushData data)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(data.FromName, data.UserName));
            message.To.Add(new MailboxAddress(data.ToName, data.ToEmailAddress));
            message.Subject = data.Subject;

            message.Body = new TextPart("plain")
            {
                Text = data.Content
            };

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
    }
}
