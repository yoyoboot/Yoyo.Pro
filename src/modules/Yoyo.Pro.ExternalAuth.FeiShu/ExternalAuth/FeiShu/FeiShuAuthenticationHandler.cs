using Yoyo.Pro.ExternalAuth.OAuth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;

namespace Yoyo.Pro.ExternalAuth.FeiShu
{
    public class FeiShuAuthenticationHandler : OAuthProviderHandler<FeiShuProviderInfo, FeiShuAuthenticationOptions>
    {
        public FeiShuAuthenticationHandler(IServiceProvider serviceProvider, IOptionsMonitor<FeiShuAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(serviceProvider, options, logger, encoder, clock)
        {
        }


        protected override async Task<AuthenticationTicket> CreateTicketAsync(
          [NotNull] ClaimsIdentity identity,
          [NotNull] AuthenticationProperties properties,
          [NotNull] OAuthTokenResponse tokens)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, Options.UserInformationEndpoint))
            {
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);


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
                        var dataJson = payload.RootElement.GetString("data");
                        payload = JsonDocument.Parse(dataJson);

                        this.Logger.LogInformation("FeiShu 用户信息：" + payload.RootElement);

                        var principal = new ClaimsPrincipal(identity);
                        var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, payload.RootElement);
                        context.RunClaimActions();

                        await Events.CreatingTicket(context);
                        return new AuthenticationTicket(context.Principal!, context.Properties, Scheme.Name);

                    }
                    catch (Exception ex)
                    {
                        throw new HttpRequestException($"未能检索飞书的用户信息,请检查参数是否正确。{responseJson}", ex);
                    }


                }
            }
        }

        protected override async Task<FeiShuProviderInfo> GetExternalAuthProviderInfo()
        {
            var externalAuthProviderInfo = await _externalAuthProviderInfoStore.Value.GetProviderInfo(this.Scheme.Name);
            return FeiShuProviderInfo.CreateByInfo(externalAuthProviderInfo);
        }

        protected override void ConfigureOptions()
        {
            base.ConfigureOptions();

            ConfigureOptionAction?.Invoke(this.Options, this.ExternalAuthProviderInfo);
        }

        public static Action<FeiShuAuthenticationOptions, FeiShuProviderInfo> ConfigureOptionAction { get; set; } = (options, providerInfo) =>
        {

        };
    }
}
