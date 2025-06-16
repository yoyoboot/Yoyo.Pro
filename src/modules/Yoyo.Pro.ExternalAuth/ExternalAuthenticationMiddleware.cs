using Yoyo.Pro.ExternalAuth;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.Extensions.DependencyInjection;

using System.Threading.Tasks;

namespace Yoyo.Pro
{
    public class ExternalAuthenticationMiddleware : IMiddleware
    {
        public ExternalAuthenticationMiddleware(IAuthenticationSchemeProvider schemes)
        {
            Schemes = schemes;
        }

        /// <summary>
        /// Gets or sets the <see cref="IAuthenticationSchemeProvider"/>.
        /// </summary>
        public IAuthenticationSchemeProvider Schemes { get; set; }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            context.Features.Set<IAuthenticationFeature>(new AuthenticationFeature
            {
                OriginalPath = context.Request.Path,
                OriginalPathBase = context.Request.PathBase
            });

            // Give any IAuthenticationRequestHandler schemes a chance to handle the request
            var handlers = context.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
            var externalAuthConfiguration = context.RequestServices.GetRequiredService<IExternalAuthConfiguration>();

            foreach (var scheme in await Schemes.GetRequestHandlerSchemesAsync())
            {
                // 第三方扩展登录，不走handler
                if (!externalAuthConfiguration.TryGetProviderInfo(scheme.Name, out var providerInfo))
                {
                    // 尝试处理
                    var handler = await handlers.GetHandlerAsync(context, scheme.Name) as IAuthenticationRequestHandler;
                    if (handler != null && await handler.HandleRequestAsync())
                    {
                        return;
                    }
                }
            }

            var defaultAuthenticate = await Schemes.GetDefaultAuthenticateSchemeAsync();
            if (defaultAuthenticate != null)
            {
                var result = await context.AuthenticateAsync(defaultAuthenticate.Name);
                if (result?.Principal != null)
                {
                    context.User = result.Principal;
                }
                if (result?.Succeeded ?? false)
                {
                    var authFeatures = new ExternalAuthenticationFeatures(result);
                    context.Features.Set<IHttpAuthenticationFeature>(authFeatures);
                    context.Features.Set<IAuthenticateResultFeature>(authFeatures);
                }
            }

            await next(context);
        }
    }
}
