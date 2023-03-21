// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using Abp.Extensions;
using Yoyo.Pro.Runtime.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Yoyo.Pro
{
    public static class YoyoProRuntimeSecurityServiceCollectionExtensions
    {
        /// <summary>
        /// 添加字符串加密服务
        /// </summary>
        /// <param name="services">服务注册器</param>
        /// <param name="configuration">配置项</param>
        /// <param name="configAction">选项配置方法</param>
        /// <returns></returns>
        public static IServiceCollection AddYoyoProStringEncryption(this IServiceCollection services, IConfiguration configuration, Action<AbpStringEncryptionOptions> configAction = null)
        {
            services.Configure<AbpStringEncryptionOptions>(options =>
             {
                 if (configAction != null)
                 {
                     configAction.Invoke(options);
                     return;
                 }


                 var keySize = configuration["StringEncryption:KeySize"];
                 if (!keySize.IsNullOrWhiteSpace())
                 {
                     if (int.TryParse(keySize, out var intValue))
                     {
                         options.Keysize = intValue;
                     }
                 }

                 var defaultPassPhrase = configuration["StringEncryption:DefaultPassPhrase"];
                 if (!defaultPassPhrase.IsNullOrWhiteSpace())
                 {
                     options.DefaultPassPhrase = defaultPassPhrase;
                 }

                 var initVectorBytes = configuration["StringEncryption:InitVectorBytes"];
                 if (!initVectorBytes.IsNullOrWhiteSpace())
                 {
                     options.InitVectorBytes = Encoding.ASCII.GetBytes(initVectorBytes); ;
                 }

                 var defaultSalt = configuration["StringEncryption:DefaultSalt"];
                 if (!defaultSalt.IsNullOrWhiteSpace())
                 {
                     options.DefaultSalt = Encoding.ASCII.GetBytes(defaultSalt); ;
                 }
             });



            return services;
        }
    }
}
