// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using Abp;
using Abp.Auditing;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Modules;
using Castle.MicroKernel.Registration;
using Yoyo.Pro.Auditing;
using Yoyo.Pro.Configuration;

namespace Yoyo.Pro
{
    [DependsOn(
        typeof(YoyoProKernelModule)
        )]
    public sealed class YoyoProAuditingModule : AbpModule
    {
        public override void PreInitialize()
        {
            // 注册 IYoyoProAuditingConfiguration 实现
            if (!IocManager.IsRegistered<IYoyoProAuditingConfiguration>())
            {
                IocManager.IocContainer.Register(
                       Component.For<IYoyoProAuditingConfiguration>()
                           .ImplementedBy<YoyoProAuditingConfiguration>()
                           .LifestyleSingleton()
                           .IsDefault()
                   );
            }
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(this.GetType().Assembly);


            var YoyoProAuditingConfiguration = IocManager.Resolve<IYoyoProAuditingConfiguration>();

            // 注册仓储
            IocManager.Register(
                typeof(IAuditLogRepository<>).MakeGenericType(YoyoProAuditingConfiguration.AuditLogType),
                YoyoProAuditingConfiguration.AuditLogRepositoryType,
                Abp.Dependency.DependencyLifeStyle.Transient
                );


            // 替换 IAuditingStore 实现
            if (IocManager.IsRegistered<IAuditingStore>())
            {
                IocManager.IocContainer.Register(
                       Component.For<IAuditingStore>()
                           .ImplementedBy(typeof(YoyoProAuditingStore<>).MakeGenericType(YoyoProAuditingConfiguration.AuditLogType))
                           .LifestyleTransient()
                           .IsDefault()
                   );
            }

        }


        public override void PostInitialize()
        {
            var aaa = IocManager.Resolve<IAuditingStore>();
        }
    }
}
