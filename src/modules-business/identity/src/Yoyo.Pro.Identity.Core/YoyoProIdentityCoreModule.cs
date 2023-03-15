using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Modules;
using Abp.Dependency;
using Abp.Reflection.Extensions;
using Abp.Zero.Configuration;
using Abp.Configuration.Startup;
using Yoyo.Pro;
using Yoyo.Pro.Localization;
using Abp.IO;
using Yoyo.Pro.AppProFolders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Yoyo.Pro
{
    [DependsOn(typeof(YoyoProCoreModule))]
    public class YoyoProIdentityCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Localization.AddYoyoProIdentityLocalization();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(this.GetType().GetAssembly());
        }

        public override void PostInitialize()
        {

        }

        public override void Shutdown()
        {
        }



    }
}
