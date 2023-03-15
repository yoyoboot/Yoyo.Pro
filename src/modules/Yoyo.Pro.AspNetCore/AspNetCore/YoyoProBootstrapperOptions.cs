using System;
using Abp;
using Microsoft.Extensions.DependencyInjection;

namespace Yoyo.Pro
{
    /// <summary>
    /// YoyoPro框架 启动配置
    /// </summary>
    public class YoyoProBootstrapperOptions
    {
        /// <summary>
        /// abp启动配置选项
        /// </summary>
        public Action<AbpBootstrapperOptions> AbpOptionsAction { get; set; }

        /// <summary>
        /// abp配置asp.net core服务之后
        /// </summary>
        public Action<IServiceCollection> AbpConfigureAspNetCoreAfter { get; set; }

    }
}
