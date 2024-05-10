using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp;
using Abp.AspNetCore;
using Abp.Modules;
using Castle.Windsor.MsDependencyInjection;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Yoyo.Pro
{
    public static class YoyoProServiceCollectionExtensions
    {
        /// <summary>
        /// 将 YoyoPro 集成到 Asp.Net Core
        /// </summary>
        /// <typeparam name="TStartupModule"></typeparam>
        /// <param name="services">服务注册器</param>
        /// <param name="optionsAction">配置选项</param>
        /// <param name="removeConventionalInterceptors">移除常规的拦截器</param>
        /// <returns></returns>
        public static IServiceProvider AddYoyoPro<TStartupModule>(this IServiceCollection services,
            [CanBeNull] Action<YoyoProBootstrapperOptions> optionsAction = null, bool removeConventionalInterceptors = true)
            where TStartupModule : AbpModule
        {
            var options = new YoyoProBootstrapperOptions();

            optionsAction?.Invoke(options);

            services.AddAbpWithoutCreatingServiceProvider<TStartupModule>(options.AbpOptionsAction, removeConventionalInterceptors);

            var abpBootstrapper = services.GetSingletonServiceOrNull<AbpBootstrapper>();

            options?.AbpConfigureAspNetCoreAfter?.Invoke(services);

            return WindsorRegistrationHelper.CreateServiceProvider(abpBootstrapper.IocManager.IocContainer, services);
        }

        /// <summary>
        /// 将 YoyoPro 集成到 Asp.Net Core
        /// </summary>
        /// <typeparam name="TStartupModule"></typeparam>
        /// <param name="services">服务注册器</param>
        /// <param name="optionsAction">配置选项</param>
        /// <param name="removeConventionalInterceptors">移除常规的拦截器</param>
        /// <returns></returns>
        public static void AddYoyoProWithoutCreatingServiceProvider<TStartupModule>(this IServiceCollection services,
            [CanBeNull] Action<YoyoProBootstrapperOptions> optionsAction = null, bool removeConventionalInterceptors = true)
            where TStartupModule : AbpModule
        {
            var options = new YoyoProBootstrapperOptions();

            optionsAction?.Invoke(options);

            services.AddAbpWithoutCreatingServiceProvider<TStartupModule>(options.AbpOptionsAction, removeConventionalInterceptors);

            var abpBootstrapper = services.GetSingletonServiceOrNull<AbpBootstrapper>();

            options?.AbpConfigureAspNetCoreAfter?.Invoke(services);
        }
    }
}
