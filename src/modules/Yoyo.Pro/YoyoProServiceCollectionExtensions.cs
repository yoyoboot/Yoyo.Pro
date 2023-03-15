// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using Abp.Reflection;
using Yoyo.Pro.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Yoyo.Pro
{
    public static class YoyoProServiceCollectionExtensions
    {
        /// <summary>
        /// 添加YoyoPro实现的TypeFinder
        /// </summary>
        /// <param name="services"></param>
        /// <param name="optionsAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddYoyoProTypeFinder(this IServiceCollection services, Action<TypeFinderOptions> optionsAction)
        {
            services.Configure<TypeFinderOptions>(optionsAction);
            services.AddSingleton<ITypeFinder, YoyoProTypeFinder>();

            return services;
        }
    }
}
