// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
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
        typeof(YoyoProAuditingModule)
        )]
    public sealed class YoyoProAuditingMongoModule : AbpModule
    {
        public override void PreInitialize()
        {

        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(this.GetType().Assembly);

            if (!IocManager.IsRegistered(typeof(IMongoAuditLogRepositoryIndexCheck<>)))
            {
                IocManager.Register(
                    typeof(IMongoAuditLogRepositoryIndexCheck<>),
                    typeof(MongoAuditLogRepositoryIndexCheck<>),
                    DependencyLifeStyle.Singleton
                    );
            }
        }


        public override void PostInitialize()
        {

        }
    }
}
