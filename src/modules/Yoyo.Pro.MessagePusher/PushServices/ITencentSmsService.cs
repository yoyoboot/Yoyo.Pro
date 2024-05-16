// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Castle.Core.Logging;
using TencentCloud.Common;
using TencentCloud.Common.Profile;
using TencentCloud.Sms.V20210111;
using TencentCloud.Sms.V20210111.Models;
using Yoyo.Pro.RequestData;

namespace Yoyo.Pro.PushServices
{
    /// <summary>
    /// 腾讯云短信服务接口
    /// </summary>
    public interface ITencentSmsService
    {
        Task<SendSmsResponse> SendMessage(TencentSmsMessagePushData data);
    }
    /// <summary>
    /// 腾讯云短信服务服务
    /// </summary>
    public class TencentSmsService : ITencentSmsService
    {
        private readonly ILogger _logger;

        public TencentSmsService(ILogger logger = null)
        {
            _logger = logger;
        }

        /// <summary>
        /// 腾讯云短信通知服务
        /// 官方地址：https://cloud.tencent.com/document/product/382/52071
        /// </summary>
        /// <param name="data"></param>
        public async Task<SendSmsResponse> SendMessage(TencentSmsMessagePushData data)
        {
            try
            {
                Credential cred = new Credential
                {
                    SecretId = data.SecretId,
                    SecretKey = data.SecretKey,
                };

                ClientProfile clientProfile = new ClientProfile();
                clientProfile.SignMethod = ClientProfile.SIGN_TC3SHA256;
                HttpProfile httpProfile = new HttpProfile();
                httpProfile.Timeout = 10;
                httpProfile.Endpoint = data.Endpoint;
                SmsClient client = new SmsClient(cred, "ap-guangzhou", clientProfile);
                SendSmsRequest req = new SendSmsRequest();
                req.SmsSdkAppId = data.SmsSdkAppId;
                req.SignName = data.SignName;
                req.TemplateId = data.TemplateId;
                req.TemplateParamSet = data.TemplateParamSet;
                req.PhoneNumberSet = data.PhoneNumberSet;
                SendSmsResponse resp = await client.SendSms(req);
                return resp;
            }
            catch (Exception e)
            {
                _logger?.Error(e.Message);
                throw;
            }
        }
    }
}
