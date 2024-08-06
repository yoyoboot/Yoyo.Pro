// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Yoyo.Pro.RequestData;

namespace Yoyo.Pro.PushServices
{
    /// <summary>
    /// 华为云短信通知接口
    /// </summary>
    public interface IHuaweiSmsService
    {
        Task<HttpResponseMessage> SendMessage(HuaweiSmsMessagePushData data);
    }
    /// <summary>
    /// 华为云短信通知服务
    /// </summary>
    public class HuaweiSmsService : IHuaweiSmsService
    {
        protected readonly IHttpClientFactory _httpClientFactory;
        protected readonly ILogger _logger;

        public HuaweiSmsService(IHttpClientFactory httpClientFactory, ILogger logger = null)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }
        /// <summary>
        /// 华为云短信通知服务
        /// 官方地址：https://support.huaweicloud.com/api-msgsms/sms_05_0001.html
        /// </summary>
        /// <param name="data">华为云短信通知内容</param>
        public virtual async Task<HttpResponseMessage> SendMessage(HuaweiSmsMessagePushData data)
        {
            string apiAddress = data.ApiAddress; //APP接入地址(在控制台"应用管理"页面获取)+接口访问URI
            string appKey = data.AppKey; //APP_Key
            string appSecret = data.AppSecret; //APP_Secret
            string sender = data.Sender; //国内短信签名通道号或国际/港澳台短信通道号
            string templateId = data.TemplateId; //模板ID

            string receiver = data.Receiver;
            string templateParas = data.TemplateParas;

            try
            {
                //为防止因HTTPS证书认证失败造成API调用失败,需要先忽略证书信任问题
                var client = _httpClientFactory.CreateClient();
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                //请求Headers
                client.DefaultRequestHeaders.Add("Authorization", "WSSE realm=\"SDP\",profile=\"UsernameToken\",type=\"Appkey\"");
                client.DefaultRequestHeaders.Add("X-WSSE", BuildWSSEHeader(appKey, appSecret));
                //请求Body
                var body = new Dictionary<string, string>() {
                    {"from", sender},
                    {"to", receiver},
                    {"templateId", templateId},
                    {"templateParas", templateParas},
                    //{"signature", signature} //使用国内短信通用模板时,必须填写签名名称
                };
                HttpContent content = new FormUrlEncodedContent(body);
                var response = await client.PostAsync(apiAddress, content);

                var res = await response.Content.ReadAsStringAsync();

                return response;
            }
            catch (Exception e)
            {
                _logger?.Error(e.Message);
                throw;
            }
        }
        /// <summary>
        /// 构造X-WSSE参数值
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        protected virtual string BuildWSSEHeader(string appKey, string appSecret)
        {
            string now = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ"); //Created
            string nonce = Guid.NewGuid().ToString().Replace("-", ""); //Nonce

            byte[] material = Encoding.UTF8.GetBytes(nonce + now + appSecret);
            byte[] hashed = SHA256.Create().ComputeHash(material);
            string hexdigest = BitConverter.ToString(hashed).Replace("-", "");
            string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(hexdigest)); //PasswordDigest

            return string.Format("UsernameToken Username=\"{0}\",PasswordDigest=\"{1}\",Nonce=\"{2}\",Created=\"{3}\"",
                            appKey, base64, nonce, now);
        }
    }
}
