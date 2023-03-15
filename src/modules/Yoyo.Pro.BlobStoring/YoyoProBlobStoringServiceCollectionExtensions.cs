// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using Abp;
using JetBrains.Annotations;
using Yoyo.Pro.BlobStoring;
using Microsoft.Extensions.DependencyInjection;

namespace Yoyo.Pro
{
    public static class YoyoProBlobStoringServiceCollectionExtensions
    {

        /// <summary>
        /// 添加并配置 YoyoPro 实现的 BlobStoring
        /// </summary>
        /// <param name="services">服务注册器</param>
        /// <param name="optionsAction">配置选项</param>
        /// <returns></returns>
        public static IServiceCollection AddYoyoProBlobStoring(this IServiceCollection services, [CanBeNull] Action<AbpBlobStoringOptions> optionsAction)
        {
            Check.NotNull(optionsAction, nameof(optionsAction));

            services.Configure<AbpBlobStoringOptions>((options) =>
            {
                optionsAction?.Invoke(options);
            });

            return services;
        }
    }
}
