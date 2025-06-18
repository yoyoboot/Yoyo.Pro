using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using Abp;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
            using (var request = new HttpRequestMessage(HttpMethod.Get, Options.UserInformationEndpoint))
            {
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Add("x-acs-dingtalk-access-token", tokens.AccessToken);
                //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);


                using (var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        await LoggingExtensions.UserProfileErrorAsync(Logger, response, Context.RequestAborted);
                        throw new HttpRequestException("An error occurred while retrieving the user profile.");
                    }

                    var responseJson = await response.Content.ReadAsStringAsync(Context.RequestAborted);
                    try
                    {
                        var payload = JsonDocument.Parse(responseJson);
                        var principal = new ClaimsPrincipal(identity);
                        var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, payload.RootElement);
                        context.RunClaimActions();

                        await Events.CreatingTicket(context);
                        return new AuthenticationTicket(context.Principal!, context.Properties, Scheme.Name);

                    }
                    catch (Exception ex)
                    {
                        throw new HttpRequestException($"未能检索钉钉的用户信息,请检查参数是否正确。{responseJson}", ex);
                    }


                }
            }
        }

        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync(OAuthCodeExchangeContext context)
        {
            var content = new Dictionary<string, string>
            {
                ["clientId"] = this.Options.ClientId,
                ["clientSecret"] = this.Options.ClientSecret,
                ["code"] = context.Code,
                ["grantType"] = "authorization_code",
            };

            var requestUri = this.Options.TokenEndpoint;
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);
            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpRequestMessage.Content = JsonContent.Create(content);
            var response = await this.Backchannel.SendAsync(httpRequestMessage, this.Context.RequestAborted);
            var responseString = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var jsonDocument = JsonDocument.Parse(responseString);
                if (jsonDocument.RootElement.GetString("accessToken").HasValue())
                {
                    var dataMap = new Dictionary<string, string>
                    {
                        ["access_token"] = jsonDocument.RootElement.GetString("accessToken"),
                        ["refresh_token"] = jsonDocument.RootElement.GetString("refreshToken"),
                        ["expires_in"] = jsonDocument.RootElement.GetString("expireIn"),
                    };
                    var resJsonDocument = JsonDocument.Parse(JsonSerializer.Serialize(dataMap));

                    var res = OAuthTokenResponse.Success(resJsonDocument);
                    return res;
                }
            }

            var ex = new Exception("使用code获取accessToken信息失败，content：" + responseString ?? string.Empty);
            return OAuthTokenResponse.Failed(ex);
        }

        protected override string BuildChallengeUrl(AuthenticationProperties properties, [NotNull] string redirectUri)
        {
            if (this._externalAuthOptions.Value.AlwaysHttps)
            {
                redirectUri = redirectUri?.Replace("http://", "https://");
            }

            var value = this.Options.StateDataFormat.Protect(properties);
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                ["scope"] = FormatScope(),
                ["response_type"] = "code",
                ["redirect_uri"] = redirectUri,
                ["state"] = value,
                ["client_id"] = this.Options.ClientSecret,
                ["prompt"] = "consent"
            };
            string parameter = properties.GetParameter<string>("loginTmpCode");
            if (!string.IsNullOrWhiteSpace(parameter))
            {
                dictionary.Add("loginTmpCode", parameter);
            }

            var redirectUri2 = QueryHelpers.AddQueryString(this.Options.AuthorizationEndpoint, dictionary);
            return redirectUri2;
        }

        protected override string FormatScope()
        {
            return string.Join(",", this.Options.Scope);
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
            options.ClientId = providerInfo.ClientId;
            options.ClientSecret = providerInfo.ClientSecret;

            options.ClaimActions.Clear();

            options.ClaimActions.MapJsonKey(Claims.UnionId, "unionId");
            options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "unionId");
            options.ClaimActions.MapJsonKey(ClaimTypes.Name, "nick");
            options.ClaimActions.MapJsonKey(ClaimTypes.MobilePhone, "mobile");
            options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
        };
    }
}
