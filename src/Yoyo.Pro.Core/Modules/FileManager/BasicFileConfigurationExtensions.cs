// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Dependency;
using Castle.MicroKernel.Registration;
using Yoyo.Pro.Modules.FileManager.DomainService;

namespace Yoyo.Pro.Modules.FileManager
{
    public static class BasicFileConfigurationExtensions
    {
        /// <summary>
        /// 添加基础的文件管理模块实现
        /// </summary>
        /// <typeparam name="TFileManager"></typeparam>
        /// <param name="iocManager"></param>
        /// <returns></returns>
        public static IIocManager UseBasicFileManagerModule<TFileManager>(this IIocManager iocManager)
            where TFileManager : IBasicFileManager
        {
            if (!iocManager.IsRegistered<IBasicFileManager>())
            {
                iocManager.IocContainer.Register(
                        Component.For<IBasicFileManager>()
                        .UsingFactoryMethod(kernel => kernel.Resolve<TFileManager>())
                        .LifestyleTransient()
                        .IsDefault()
                    );
            }
            return iocManager;
        }
    }
}
