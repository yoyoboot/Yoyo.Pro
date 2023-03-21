// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Abp;
using Abp.Modules;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Yoyo.Pro
{
    public class YoyoProBootstrapper
    {
        public static void Run<TStartupModule>(IYoyoProStartup startup)
            where TStartupModule : AbpModule
        {
            Check.NotNull(startup, nameof(startup));

            IServiceCollection services = new ServiceCollection();
            // 配置服务
            startup.ConfigurationServices(services);


            // 注册添加abp启动器
            var abpBootstrapper = AbpBootstrapper
                .Create<TStartupModule>(startup.ConfigureAbp);
            services.AddSingleton(abpBootstrapper);

            // 服务配置结束
            var serviceProvider = WindsorRegistrationHelper.CreateServiceProvider(
                abpBootstrapper.IocManager.IocContainer,
                services
                );

            startup.Configure(serviceProvider);
        }
    }
}
