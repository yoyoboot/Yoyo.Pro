// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Castle.Core.Logging;
using Yoyo.Pro.RequestData;

namespace Yoyo.Pro.PushServices
{
    /// <summary>
    /// 机器人消息通知接口
    /// </summary>
    public interface IRobotPushService
    {
        Task<string> SendMessage(RobotMessagePushData data);
    }
    /// <summary>
    /// 机器人消息通知服务
    /// </summary>
    public class RobotPushService : IRobotPushService
    {
        private readonly IRobotPushHttpApi _messagePushService;
        private readonly ILogger _logger;

        public RobotPushService(IRobotPushHttpApi messagePushService, ILogger logger = null)
        {
            _messagePushService = messagePushService;
            _logger = logger;
        }
        /// <summary>
        /// 机器人消息通知服务
        /// 官方地址：https://cloud.tencent.com/document/product/1263/71892
        /// </summary>
        /// <param name="data">消息通知内容</param>
        /// <returns></returns>
        public virtual async Task<string> SendMessage(RobotMessagePushData data)
        {
            var contentData = default(object);
            switch (data.PushChannelType)
            {
                case PushChannelType.RobotFeishu:
                    contentData = new
                    {
                        msg_type = data.FeiShuContent.msg_Type,
                        content = data.FeiShuContent.content
                    };
                    if (!string.IsNullOrWhiteSpace(data.Secret))
                    {
                        TimeSpan ts = GetTimeSpan();
                        string timestamp = Convert.ToInt64(ts.TotalSeconds).ToString();
                        contentData = new
                        {
                            timestamp = timestamp,
                            sign = GetFeishuSign(timestamp, data.Secret),
                            msg_type = data.FeiShuContent.msg_Type,
                            content = data.FeiShuContent.content
                        };
                    }
                    break;
                case PushChannelType.RobotWechat:
                    contentData = new
                    {
                        msgtype = data.WechatContent.msg_Type,
                        text = data.WechatContent.text
                    };
                    break;
                case PushChannelType.RobotDingding:
                    contentData = new
                    {
                        msgtype = data.DingdingContent.msg_Type,
                        text = data.DingdingContent.text
                    };
                    if (!string.IsNullOrWhiteSpace(data.Secret))
                    {
                        TimeSpan ts = GetTimeSpan();
                        string timestamp = Convert.ToInt64(ts.TotalMilliseconds).ToString();
                        var sign = GetDingdingSign(timestamp, data.Secret);
                        data.WebHookUrl = $"{data.WebHookUrl}&timestamp={timestamp}&sign={sign}";
                    }
                    break;
            }

            try
            {
                var result = await _messagePushService.MessagePush(data.WebHookUrl, contentData);
                return result;
            }
            catch (Exception ex)
            {
                _logger?.Error(ex.Message);
                throw;
            }

        }

        #region Private
        /// <summary>
        /// 获取钉钉机器人签名
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        protected virtual string GetDingdingSign(string timeStamp, string secret)
        {
            string sign = null;
            var encoding = new UTF8Encoding();
            byte[] signBytes = encoding.GetBytes($"{timeStamp}\n{secret}");
            byte[] secretByte = encoding.GetBytes(secret);
            using (var hmacsha256 = new HMACSHA256(secretByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(signBytes);
                sign = HttpUtility.UrlEncode(Convert.ToBase64String(hashmessage));
            }
            return sign;
        }
        /// <summary>
        /// 获取飞书机器人签名
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        protected virtual string GetFeishuSign(string timeStamp, string secret)
        {
            string sign = null;
            var encoding = new UTF8Encoding();
            byte[] signBytes = encoding.GetBytes($"{timeStamp}\n{secret}");
            using (var hmacsha256 = new HMACSHA256(signBytes))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(new byte[] { });
                sign = Convert.ToBase64String(hashmessage);
            }
            return sign;
        }
        protected virtual TimeSpan GetTimeSpan()
        {
            return DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        }

        #endregion
    }
}
