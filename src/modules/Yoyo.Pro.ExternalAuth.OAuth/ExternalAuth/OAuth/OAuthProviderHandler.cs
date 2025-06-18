using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Yoyo.Pro.ExternalAuth.OAuth
{
    public abstract class OAuthProviderHandler<TExternalAuthProviderInfo, TOptions> : OAuthHandler<TOptions>
        where TExternalAuthProviderInfo : OAuthProviderInfo, new()
        where TOptions : OAuthOptions, new()
    {

        protected readonly IServiceProvider _serviceProvider;
        protected readonly Lazy<IAbpSession> _session;
        protected readonly Lazy<ExternalAuthOptions> _externalAuthOptions;
        protected readonly Lazy<IExternalAuthJwtTokenService> _externalAuthJwtTokenService;
        protected readonly Lazy<IExternalAuthProviderInfoStore> _externalAuthProviderInfoStore;


        protected string TenantId => _session.Value.TenantId;
        protected string ProviderName => this.Scheme.Name;

        protected TExternalAuthProviderInfo ExternalAuthProviderInfo { get; set; }

        public OAuthProviderHandler(IServiceProvider serviceProvider, IOptionsMonitor<TOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            _serviceProvider = serviceProvider;
            _session = this._serviceProvider.GetLazy<IAbpSession>();
            _externalAuthOptions = this._serviceProvider.GetLazy<ExternalAuthOptions>();
            _externalAuthJwtTokenService = this._serviceProvider.GetLazy<IExternalAuthJwtTokenService>();
            _externalAuthProviderInfoStore = this._serviceProvider.GetLazy<IExternalAuthProviderInfoStore>();
        }

        public override Task<bool> ShouldHandleRequestAsync()
        {
            var pathMatched = this.Options.CallbackPath.Value.EndsWith(Request.Path);

            var providerInfoNameMatched = this.ExternalAuthProviderInfo.Name == this.Request.Cookies["ExternalAuthProvider"];

            return Task.FromResult(pathMatched && providerInfoNameMatched);
        }


        protected override async Task InitializeHandlerAsync()
        {
            await base.InitializeHandlerAsync();

            // 配置公共信息
            this.ExternalAuthProviderInfo = await this.GetExternalAuthProviderInfo();
            this.ConfigureOptions();

            // 初始化事件信息
            // ======== 登录token验证成功 ========
            this.Events.OnTicketReceived = async (e) =>
            {
                // 生成token
                var accessToken = _externalAuthJwtTokenService.Value.GenerateToken(e.Principal);

                // 获取provider key
                var providerClaim = e.Principal.GetClaimByMapping(this.ExternalAuthProviderInfo.ClaimsMapping, ClaimTypes.NameIdentifier);
                var providerKey = providerClaim.Value;

                // 生成重定向地址
                var redirectUri = $"{this._externalAuthOptions.Value.SignInSuccessRedirectUri}?tenantId={this.TenantId ?? string.Empty}&authProvider={this.ProviderName}&providerKey={providerKey}&providerAccessCode={accessToken}";
                redirectUri = redirectUri.ReplaceUrlTenancyName(this.ExternalAuthProviderInfo.TenancyName);

                // 重定向
                e.Response.Redirect(redirectUri);
                await e.Response.CompleteAsync();

                // 标识已经处理过了
                e.HandleResponse();
            };
        }

        protected override async Task<HandleRequestResult> HandleRemoteAuthenticateAsync()
        {
            return await base.HandleRemoteAuthenticateAsync();
        }

        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync(OAuthCodeExchangeContext context)
        {

            // 配置公共信息
            this.ExternalAuthProviderInfo = await this.GetExternalAuthProviderInfo();
            this.ConfigureOptions();

            var res = await base.ExchangeCodeAsync(context);
            return res;
        }

        protected virtual void ConfigureOptions()
        {
            this.Options.CallbackPath = this._externalAuthOptions.Value.CallbackPath;

            this.Options.ClientId = this.ExternalAuthProviderInfo.ClientId;
            this.Options.ClientSecret = this.ExternalAuthProviderInfo.ClientSecret;

            this.Options.AuthorizationEndpoint = this.ExternalAuthProviderInfo.AuthorizationEndpoint;
            this.Options.TokenEndpoint = this.ExternalAuthProviderInfo.TokenEndpoint;
            this.Options.UserInformationEndpoint = this.ExternalAuthProviderInfo.UserInformationEndpoint;
            this.Options.CorrelationCookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
        }

        protected abstract Task<TExternalAuthProviderInfo> GetExternalAuthProviderInfo();
    }
}
