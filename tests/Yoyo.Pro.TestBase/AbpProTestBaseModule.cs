using Abp;
using Abp.Dependency;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.TestBase;
using Castle.Windsor.Installer;
using System;
using Xunit;
using System.Reflection;

namespace Yoyo.Pro
{
    [DependsOn(
 
        typeof(YoyoProApplicationModule),
        typeof(YoyoProEntityFrameworkCoreModule),
        typeof(AbpTestBaseModule)
        )]
    public class AbpProTestBaseModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.EventBus.UseDefaultEventBus = false;
            Configuration.DefaultNameOrConnectionString = "Default";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
