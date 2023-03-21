// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.AspNetCore.Builder;

namespace Yoyo.Pro.Security
{
    /// <summary>
    /// 中间件拓展方法
    /// </summary>
    public static class SecurityHeadersMiddlewareExtensions
    {
        public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app, Action<SecurityHeadersBuilder> builderAction)
        {
            var builder = new SecurityHeadersBuilder();
            builderAction?.Invoke(builder);
            var policy = builder.Build();

            return app.UseMiddleware<SecurityHeadersMiddleware>(policy);
        }
    }
}
