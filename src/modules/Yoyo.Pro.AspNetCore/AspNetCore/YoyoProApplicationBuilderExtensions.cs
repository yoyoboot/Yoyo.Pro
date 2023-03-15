// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AspNetCore;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;

namespace Yoyo.Pro
{
    public static class YoyoProApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseYoyoPro([NotNull] this IApplicationBuilder app, Action<AbpApplicationBuilderOptions> optionsAction)
        {
            app.UseAbp((options) =>
            {
                options.UseAbpRequestLocalization = false;

                optionsAction?.Invoke(options);
            });

            return app;
        }
    }
}
