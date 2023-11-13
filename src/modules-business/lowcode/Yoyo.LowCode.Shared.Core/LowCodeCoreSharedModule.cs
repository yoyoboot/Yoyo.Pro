// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Abp.Modules;
using Abp.Reflection.Extensions;
using Yoyo.LowCode.Localization;
using Yoyo.Pro;

namespace Yoyo.LowCode
{
    [DependsOn(typeof(YoyoProIdentityCoreModule))]
    public class LowCodeCoreSharedModule : AbpModule
    {
        public override void PreInitialize()
        {
            // 本地化
            Configuration.Localization.AddLowCodeLocalization();
        }

        public override void Initialize()
        {
            // 注册程序集中的依赖
            IocManager.RegisterAssemblyByConvention(this.GetType().GetAssembly());
        }

        public override void PostInitialize()
        {
        }
    }
}
