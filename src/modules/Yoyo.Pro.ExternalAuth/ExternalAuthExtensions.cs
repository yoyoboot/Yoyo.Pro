using Yoyo.Pro.ExternalAuth;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using System;

namespace Yoyo.Pro
{
    public static class ExternalAuthExtensions
    {
        internal const string AuthenticationMiddlewareSetKey = "__AuthenticationMiddlewareSet";

        /// <summary>
        /// 添加扩展身份验证服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddExternalAuthentication(this AuthenticationBuilder authenticationBuilder, Action<ExternalAuthOptions> setupAction)
        {
            // 依赖的服务
            authenticationBuilder.Services.AddHttpContextAccessor();

            // 中间件
            authenticationBuilder.Services.AddTransient<ExternalAuthenticationMiddleware>();

            // 核心服务
            authenticationBuilder.Services.AddExternalAuthenticationCore(setupAction);

            return authenticationBuilder;
        }

        /// <summary>
        /// 添加扩展身份验证核心服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddExternalAuthenticationCore(this IServiceCollection services, Action<ExternalAuthOptions> setupAction = null)
        {
            // 配置选项
            var options = new ExternalAuthOptions();
            setupAction?.Invoke(options);
            services.AddSingleton<ExternalAuthOptions>(options);

            // 基本配置
            var configuration = new ExternalAuthConfiguration();
            services.AddSingleton<IExternalAuthConfiguration>(configuration);


            // 公共校验器
            services.TryAddTransient<IExternalAuthProviderApi, ExternalAuthProviderApi>();


            // 扩展登录信息存储器
            services.TryAddTransient<IExternalAuthProviderInfoStore, ExternalAuthProviderInfoStore>();


            // 内置jwt校验器和配置
            services.TryAddSingleton<IExternalAuthJwtTokenService, ExternalAuthJwtTokenService>();

            return services;
        }

        /// <summary>
        /// 添加身份验证中间件（包含扩展身份验证），等于 UseAuthentication，需要放到 UseAuthorization 之前
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseExternalAuthentication(this IApplicationBuilder app)
        {
            ArgumentNullException.ThrowIfNull(app);

            app.Properties[AuthenticationMiddlewareSetKey] = true;
            return app.UseMiddleware<ExternalAuthenticationMiddleware>();
        }
    }
}
