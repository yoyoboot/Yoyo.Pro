// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Abp;
using Microsoft.Extensions.DependencyInjection;

namespace Yoyo.Pro
{
    /// <summary>
    /// 启动接口
    /// </summary>
    public interface IYoyoProStartup
    {

        /// <summary>
        /// 配置服务
        /// </summary>
        /// <param name="services"></param>
        void ConfigurationServices(IServiceCollection services);

        /// <summary>
        /// 配置Abp
        /// </summary>
        /// <param name="options"></param>
        void ConfigureAbp(AbpBootstrapperOptions options);

        /// <summary>
        /// 应用配置
        /// </summary>
        /// <param name="serviceProvider"></param>
        void Configure(IServiceProvider serviceProvider);
    }
}
