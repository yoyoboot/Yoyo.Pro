using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

using Yoyo.Pro.ExternalAuth.OAuth;

using static Yoyo.Pro.ExternalAuth.DingTalk.DingTalkAuthenticationConstants;

namespace Yoyo.Pro.ExternalAuth.DingTalk
{
    public class DingTalkAuthenticationHandler : OAuthProviderHandler<DingTalkProviderInfo, DingTalkAuthenticationOptions>
    {
        public DingTalkAuthenticationHandler(IServiceProvider serviceProvider, IOptionsMonitor<DingTalkAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(serviceProvider, options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync(ClaimsIdentity identity, AuthenticationProperties properties, OAuthTokenResponse tokens)
        {
            var userInfo = await GetUserInfoByCode(this.Context.Request.Query["code"]);
            if (userInfo == null)
            {
                throw new HttpRequestException($"未能检索钉钉的用户信息,请检查参数是否正确。");
            }
            var content = userInfo.RootElement.GetString("user_info");

            var jsonDocument = JsonDocument.Parse(content);
            this.Logger.LogInformation("DingTalk 用户信息：" + jsonDocument.RootElement);

            #region 获取用户详细信息，暂时不用(只能获取内部员工)
            if (this.Options.IsEmployee)
            {
                var uninoid = jsonDocument.RootElement.GetString("unionid");
                var userid = await GetUserId(uninoid, tokens.AccessToken);
                if (userid == null)
                {
                    throw new HttpRequestException($"未能检索钉钉的用户id信息,请检查参数是否正确。");
                }
                jsonDocument = await GetUserInfoById(userid, tokens.AccessToken);
                this.Logger.LogInformation("用户信息：" + jsonDocument.RootElement);
            }

            #endregion


            var context = new OAuthCreatingTicketContext(new ClaimsPrincipal(identity), properties, this.Context, this.Scheme, (OAuthOptions)this.Options, this.Backchannel, tokens, jsonDocument.RootElement);
            context.RunClaimActions();
            await this.Events.CreatingTicket(context);
            return new AuthenticationTicket(context.Principal, context.Properties, this.Scheme.Name);
        }

        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync(OAuthCodeExchangeContext context)
        {
            Logger.LogInformation($"{this.GetType().Name} ExchangeCodeAsync 1");

            this.ExternalAuthProviderInfo = await this.GetExternalAuthProviderInfo();

            // 配置公共信息
            this.ConfigureOptions();

            if (this.Options.IsEmployee)
            {
                Exception ex = new Exception("换取access_token失败，content：");
                Dictionary<string, string> dictionary = new Dictionary<string, string>
                {
                    ["appkey"] = this.ExternalAuthProviderInfo.ClientId,
                    ["appsecret"] = this.ExternalAuthProviderInfo.ClientSecret,

                };
                string endpoint = QueryHelpers.AddQueryString(this.Options.TokenEndpoint, dictionary);
                HttpResponseMessage response = await this.Backchannel.GetAsync(endpoint, this.Context.RequestAborted);

                string text = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    JsonDocument jsonDocument = JsonDocument.Parse(text);
                    if (jsonDocument.RootElement.GetString("errcode") != "0")
                    {
                        ex = new Exception("换取access_token失败，content：" + text);
                        this.Logger.LogError(ex, "DingDingHandler ExchangeCodeAsync");
                        return OAuthTokenResponse.Failed(ex);
                    }
                    return OAuthTokenResponse.Success(jsonDocument);
                }


                return OAuthTokenResponse.Failed(ex);
            }
            //如果不获取内部员工无需配置获取token
            Dictionary<string, string> dictionarytoken = new Dictionary<string, string>
            {
                ["access_token"] = context.Code,
            };

            Logger.LogInformation($"{this.GetType().Name} ExchangeCodeAsync 2");

            return OAuthTokenResponse.Success(JsonDocument.Parse(JsonSerializer.Serialize(dictionarytoken)));
        }

        protected override string BuildChallengeUrl(AuthenticationProperties properties, [NotNull] string redirectUri)
        {
            string value = this.Options.StateDataFormat.Protect(properties);
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                ["appid"] = this.Options.AppId,
                ["scope"] = FormatScope(),
                ["response_type"] = "code",
                ["redirect_uri"] = redirectUri,
                ["state"] = value
            };
            string parameter = properties.GetParameter<string>("loginTmpCode");
            if (!string.IsNullOrWhiteSpace(parameter))
            {
                dictionary.Add("loginTmpCode", parameter);
            }

            redirectUri = QueryHelpers.AddQueryString(this.Options.AuthorizationEndpoint, dictionary);
            return redirectUri;
        }

        protected override string FormatScope()
        {
            return string.Join(",", this.Options.Scope);
        }
        /// <summary>
        /// 根据code获取用户信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private async Task<JsonDocument> GetUserInfoByCode(string code)
        {
            DateTimeOffset dto = new DateTimeOffset(DateTime.Now);
            var timestamp = dto.ToUnixTimeMilliseconds().ToString();

            var signature = EncryptWithSHA256(this.Options.AppSecret, timestamp);

            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                ["accessKey"] = this.Options.AppId,
                ["timestamp"] = timestamp,
                ["signature"] = signature,
            };
            Dictionary<string, string> content = new Dictionary<string, string>
            {
                ["tmp_auth_code"] = code,

            };
            string json = JsonSerializer.Serialize(content);
            StringContent stringContent = new StringContent(json);
            var requestUri = QueryHelpers.AddQueryString(this.Options.UserInformationByCodeEndpoint, dictionary);

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);
            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpRequestMessage.Content = stringContent;
            HttpResponseMessage response = await this.Backchannel.SendAsync(httpRequestMessage, this.Context.RequestAborted);
            string text = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                JsonDocument jsonDocument = JsonDocument.Parse(text);
                if (jsonDocument.RootElement.GetString("errcode") != "0")
                {
                    Exception ex = new Exception("使用code获取用户信息失败，content：" + text);
                    this.Logger.LogError(ex, "DingDingHandler ExchangeCodeAsync");
                    return null;
                }

                return jsonDocument;
            }
            return null;
        }
        /// <summary>
        /// 根据unionid获取userid
        /// </summary>
        /// <param name="unionid"></param>
        /// <param name="access_token"></param>
        /// <returns></returns>
        private async Task<string> GetUserId(string unionid, string access_token)
        {
            var dto = new DateTimeOffset(DateTime.Now);
            var timestamp = dto.ToUnixTimeMilliseconds().ToString();

            var queryStringMap = new Dictionary<string, string>
            {
                ["access_token"] = access_token,

            };
            var requestBodyMap = new Dictionary<string, string>
            {
                ["unionid"] = unionid,
            };

            var requestUri = QueryHelpers.AddQueryString(this.Options.UserIdByUnionidEndpoint, queryStringMap);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);
            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpRequestMessage.Content = new StringContent(JsonSerializer.Serialize(requestBodyMap));
            var response = await this.Backchannel.SendAsync(httpRequestMessage, this.Context.RequestAborted);
            var text = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var jsonDocument = JsonDocument.Parse(text);
                if (jsonDocument.RootElement.GetString("errcode") != "0")
                {
                    Exception ex = new Exception("使用unionid获取userid失败，content：" + text);
                    this.Logger.LogError(ex, "DingDingHandler GetUserId");
                    return null;
                }
                var result = jsonDocument.RootElement.GetString("result");

                var resultDocument = JsonDocument.Parse(result);
                var userid = resultDocument.RootElement.GetString("userid");
                return userid;
            }
            else
            {
                var ex = new Exception("使用unionid获取userid失败，content：" + text);
                this.Logger.LogError(ex, "DingDingHandler GetUserId");
            }
            return null;
        }
        /// <summary>
        /// 更具userid或者用户信息
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="access_token"></param>
        /// <returns></returns>
        private async Task<JsonDocument> GetUserInfoById(string userid, string access_token)
        {
            var dto = new DateTimeOffset(DateTime.Now);
            var timestamp = dto.ToUnixTimeMilliseconds().ToString();

            var queryStringMap = new Dictionary<string, string>
            {
                ["access_token"] = access_token,

            };
            var requestBodyMap = new Dictionary<string, string>
            {
                ["userid"] = userid,

            };

            var requestUri = QueryHelpers.AddQueryString(this.Options.UserInformationEndpoint, queryStringMap);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);
            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpRequestMessage.Content = new StringContent(JsonSerializer.Serialize(requestBodyMap));
            var response = await this.Backchannel.SendAsync(httpRequestMessage, this.Context.RequestAborted);
            var text = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var jsonDocument = JsonDocument.Parse(text);
                if (jsonDocument.RootElement.GetString("errcode") != "0")
                {
                    var ex = new Exception("使用userid获取用户信息失败，content：" + text);
                    this.Logger.LogError(ex, "DingDingHandler GetUserInfoById");
                    return null;
                }
                var result = jsonDocument.RootElement.GetString("result");

                var resultDocument = JsonDocument.Parse(result);
                return resultDocument;
            }
            return null;
        }

        /// <summary>
        /// 签名
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        protected virtual string EncryptWithSHA256(string accessKey, string timestamp)
        {
            var keyBytes = Encoding.UTF8.GetBytes(accessKey);
            var strBytes = Encoding.UTF8.GetBytes(timestamp);
            using (var hmacsha256 = new HMACSHA256(keyBytes))
            {
                var hashmessage = hmacsha256.ComputeHash(strBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }


        protected override async Task<DingTalkProviderInfo> GetExternalAuthProviderInfo()
        {
            var externalAuthProviderInfo = await _externalAuthProviderInfoStore.Value.GetProviderInfo(this.Scheme.Name);
            return DingTalkProviderInfo.CreateByInfo(externalAuthProviderInfo);
        }

        protected override void ConfigureOptions()
        {
            base.ConfigureOptions();

            ConfigureOptionAction?.Invoke(this.Options, this.ExternalAuthProviderInfo);
        }


        public static Action<DingTalkAuthenticationOptions, DingTalkProviderInfo> ConfigureOptionAction { get; set; } = (options, providerInfo) =>
        {
            options.UserInformationByCodeEndpoint = providerInfo.UserInformationByCodeEndpoint;
            options.UserIdByUnionidEndpoint = providerInfo.UserIdByUnionidEndpoint;
            options.IsEmployee = providerInfo.IsEmployee;
            options.AppId = providerInfo.AppId ?? providerInfo.ClientId;
            options.AppSecret = providerInfo.AppSecret ?? providerInfo.ClientSecret;

            options.ClaimActions.Clear();

            if (options.IsEmployee)
            {
                options.ClaimActions.MapJsonKey(Claims.UnionId, "unionid");
                options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "unionid");
                options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");

                options.ClaimActions.MapJsonKey(ClaimTypes.MobilePhone, "mobile");
            }
            else
            {
                options.ClaimActions.MapJsonKey(Claims.UnionId, "unionid");
                options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "unionid");
                options.ClaimActions.MapJsonKey(ClaimTypes.Name, "nick");
            }
        };
    }
}
