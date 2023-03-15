using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Abp.Localization;

using Yoyo.Pro.Localization;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;

namespace Yoyo.Pro
{
    public static class YoyoProAspNetCoreLocalizationExtensions
    {
        /// <summary>
        /// 添加YoyoPro的请求本地化服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddYoyoProRequestLocalization(this IServiceCollection services)
        {
            services.TryAddTransient<IRequestCultureAccessor, DefaultRequestCultureAccessor>();
            return services;
        }


        /// <summary>
        /// 启用YoyoPro的请求本地化服务
        /// </summary>
        /// <param name="app"></param>
        /// <param name="optionsAction">配置方法</param>
        /// <returns></returns>
        public static IApplicationBuilder UseYoyoProRequestLocalization(this IApplicationBuilder app, Action<RequestLocalizationOptions> optionsAction = null)
        {
            var serviceProvider = app.ApplicationServices;
            var languageManager = serviceProvider.GetService<ILanguageManager>();


            var languageInfos = languageManager.GetActiveLanguages();
            var defaultLanguage = languageInfos.FirstOrDefault(o => o.IsDefault);

            var supportedCultures = languageInfos
                .Select(o =>
                {
                    return CultureInfo.GetCultureInfo(o.Name);
                }).ToArray();

            var options = new RequestLocalizationOptions
            {
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures,
            };
            // 本地化默认使用的语言信息
            var defaultCultureInfo = supportedCultures.FirstOrDefault(o => o.Name == defaultLanguage.Name);
            if (defaultCultureInfo != null)
            {
                options.DefaultRequestCulture = new RequestCulture(
                    defaultCultureInfo,
                    defaultCultureInfo
                    );
            }

            options.RequestCultureProviders.Insert(1, new YoyoProUserRequestCultureProvider(options));


            // 自行配置
            optionsAction?.Invoke(options);


            return app.UseRequestLocalization(options);
        }
    }
}
