// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlibabaCloud.OpenApiClient.Models;
using AlibabaCloud.SDK.Dysmsapi20170525;
using AlibabaCloud.SDK.Dysmsapi20170525.Models;
using AlibabaCloud.TeaUtil.Models;
using Castle.Core.Logging;
using Tea;
using Yoyo.Pro.RequestData;

namespace Yoyo.Pro.PushServices
{

    /// <summary>
    /// 阿里云短信通知接口
    /// </summary>
    public interface IAliSmsService
    {
        Task<SendSmsResponse> SendMessage(AliSmsMessagePushData smsData);
    }
    /// <summary>
    /// 阿里云短信通知服务
    /// </summary>
    public class AliSmsService : IAliSmsService
    {
        protected readonly ILogger _logger;
        public AliSmsService(ILogger logger = null)
        {
            _logger = logger;
        }

        /// <summary>
        /// 阿里云短信通知
        /// 官方地址：https://help.aliyun.com/document_detail/419273.htm?spm=a2c4g.419298.0.0.25116cf0UnkFlr
        /// </summary>
        /// <param name="smsData">阿里云短信通知内容</param>
        public virtual async Task<SendSmsResponse> SendMessage(AliSmsMessagePushData smsData)
        {
            Client client = CreateClient(smsData.AccessKeyId, smsData.AccessKeySecret, smsData.Endpoint);
            SendSmsRequest sendSmsRequest = new SendSmsRequest
            {
                PhoneNumbers = smsData.PhoneNumbers,
                SignName = smsData.SignName,
                TemplateCode = smsData.TemplateCode,
                TemplateParam = smsData.TemplateParam,
            };
            RuntimeOptions runtime = new RuntimeOptions();
            try
            {
                // 复制代码运行请自行打印 API 的返回值
                var response = await client.SendSmsWithOptionsAsync(sendSmsRequest, runtime);
                return response;
            }
            catch (TeaException error)
            {
                // 如有需要，请打印 error
                AlibabaCloud.TeaUtil.Common.AssertAsString(error.Message);
                _logger?.Error(error.Message);
                throw;
            }
            catch (Exception _error)
            {
                TeaException error = new TeaException(new Dictionary<string, object>
                {
                    { "message", _error.Message }
                });
                // 如有需要，请打印 error
                AlibabaCloud.TeaUtil.Common.AssertAsString(error.Message);
                _logger?.Error(_error.Message);
                throw;
            }
        }
        /// <summary>
        /// 构造短信发送的Client
        /// </summary>
        /// <param name="accessKeyId"></param>
        /// <param name="accessKeySecret"></param>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        protected virtual Client CreateClient(string accessKeyId, string accessKeySecret, string endpoint)
        {
            Config config = new Config
            {
                // 必填，您的 AccessKey ID
                AccessKeyId = accessKeyId,
                // 必填，您的 AccessKey Secret
                AccessKeySecret = accessKeySecret,
            };
            // 访问的域名
            config.Endpoint = endpoint;
            return new Client(config);
        }

    }
}
