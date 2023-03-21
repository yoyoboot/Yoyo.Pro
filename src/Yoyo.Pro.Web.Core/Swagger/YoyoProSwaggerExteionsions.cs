// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Yoyo.Pro.EnumHelper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Yoyo.Pro.Swagger
{
    public static class YoyoProSwaggerExteionsions
    {
        /// <summary>
        /// 配置使用枚举处理器
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static SwaggerGenOptions UseEnumSchemaFilter(this SwaggerGenOptions options)
        {
            options.SchemaFilter<SwaggerEnumSchemaFilter>();
            return options;
        }

        /// <summary>
        /// 配置使用 <see cref="SwaggerEnumParameterFilter"/>
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static SwaggerGenOptions UseEnumParameterFilter(this SwaggerGenOptions options)
        {
            options.ParameterFilter<SwaggerEnumParameterFilter>();

            return options;
        }
        /// <summary>
        /// 配置使用 <see cref="SwaggerOperationIdFilter"/>
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static SwaggerGenOptions UseOperationIdFilter(this SwaggerGenOptions options)
        {
            options.OperationFilter<SwaggerOperationIdFilter>();

            return options;
        }

        /// <summary>
        /// 配置使用 <see cref="SwaggerOperationFilter"/>
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static SwaggerGenOptions UseOperationFilter(this SwaggerGenOptions options)
        {
            options.OperationFilter<SwaggerOperationFilter>();

            return options;
        }




        /// <summary>
        /// 触发生成swagger文档
        /// </summary>
        /// <param name="app"></param>
        /// <param name="action">初始化调用方法</param>
        /// <returns></returns>
        public static IApplicationBuilder UseSwaggerProvider
            (this IApplicationBuilder app, Action<ISwaggerProvider> action)
        {
            var swaggerProvider = app.ApplicationServices.GetService(typeof(ISwaggerProvider)) as ISwaggerProvider;

            if (swaggerProvider != null)
            {
                action?.Invoke(swaggerProvider);
            }
            return app;
        }
    }
}
