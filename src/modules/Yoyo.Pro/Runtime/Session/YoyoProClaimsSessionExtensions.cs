// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Abp.Dependency;
using Abp.Runtime.Session;
using Castle.MicroKernel.Registration;

namespace Yoyo.Pro.Runtime.Session
{
    public static class YoyoProClaimsSessionExtensions
    {
        /// <summary>
        /// 使用YoyoPro实现的AbpSession
        /// </summary>
        /// <param name="iocManager"></param>
        public static void UseYoyoProClaimsSession(this IIocManager iocManager)
        {
            if (iocManager.IsRegistered<IAbpSession>())
            {
                iocManager.IocContainer.Register(
                    Component.For<IAbpSession>()
                    .UsingFactoryMethod(kernel =>
                    {
                        return kernel.Resolve<YoyoProClaimsSession>();
                    })
                    .LifestyleTransient()
                    .IsDefault()
                    );
            }
        }
    }
}

