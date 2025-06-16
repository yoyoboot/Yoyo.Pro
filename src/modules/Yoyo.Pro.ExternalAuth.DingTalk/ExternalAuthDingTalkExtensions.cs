using Yoyo.Pro.ExternalAuth;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Yoyo.Pro.ExternalAuth.DingTalk;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Abp;


namespace Yoyo.Pro
{
    public static class ExternalAuthDingTalkExtensions
    {
        /// <summary>
        /// 添加基于 DingTalk 的身份认证服务
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="authenticationScheme"></param>
        /// <param name="providerInfo"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddDingTalkProvider(this AuthenticationBuilder builder, string authenticationScheme, DingTalkProviderInfo providerInfo)
        {
            Check.NotNull(builder, nameof(builder));
            Check.NotNullOrWhiteSpace(authenticationScheme, nameof(authenticationScheme));
            Check.NotNull(providerInfo, nameof(providerInfo));

            // 添加配置
            var externalAuthOptions = builder.Services.GetSingletonInstance<ExternalAuthOptions>();

            var externalAuthConfiguration = builder.Services.GetSingletonInstance<IExternalAuthConfiguration>();
            providerInfo.Name = authenticationScheme;
            externalAuthConfiguration.AddProviderInfo(providerInfo);

            // 添加处理服务
            return builder.AddOAuth<DingTalkAuthenticationOptions, DingTalkAuthenticationHandler>(authenticationScheme, (options) =>
              {
                  options.CallbackPath = externalAuthOptions.CallbackPath;

                  options.ClientId = providerInfo.ClientId;
                  options.ClientSecret = providerInfo.ClientSecret;

                  options.AuthorizationEndpoint = providerInfo.AuthorizationEndpoint;
                  options.TokenEndpoint = providerInfo.TokenEndpoint;
                  options.UserInformationEndpoint = providerInfo.UserInformationEndpoint;
              });
        }
    }
}
