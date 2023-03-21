// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using Abp;
using Abp.Modules;
using Abp.Runtime.Session;

namespace Yoyo.Pro
{
    [DependsOn(
        typeof(AbpKernelModule)
        )]
    public sealed class YoyoProKernelModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(this.GetType().Assembly);
        }

    }
}
