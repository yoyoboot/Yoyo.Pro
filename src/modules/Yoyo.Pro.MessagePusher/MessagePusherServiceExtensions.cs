// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Yoyo.Pro.PushServices;

namespace Yoyo.Pro
{
    public static class MessagePusherServiceExtensions
    {
        /// <summary>
        /// 添加AbpEX对于消息中心的拓展服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMessagePusherService(this IServiceCollection services)
        {
            services.TryAddTransient<IAliSmsService, AliSmsService>();
            services.TryAddTransient<ITencentSmsService, TencentSmsService>();
            services.TryAddTransient<IHuaweiSmsService, HuaweiSmsService>();
            services.TryAddTransient<IRobotPushService, RobotPushService>();
            services.TryAddTransient<IEmailPushService, EmailPushService>();
            services.AddHttpApi<IRobotPushHttpApi>();
            return services;
        }
    }
}
