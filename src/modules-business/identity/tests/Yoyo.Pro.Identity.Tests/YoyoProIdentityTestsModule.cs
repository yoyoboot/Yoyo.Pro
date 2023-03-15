using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.TestBase;
using Abp.Web;

namespace Yoyo.Pro
{
    [DependsOn(typeof(YoyoProIdentityApplicationModule), 
        typeof(YoyoProIdentityEntityFrameworkCoreModule),
        typeof(AbpTestBaseModule)
        )]
    public class YoyoProIdentityTestsModule : AbpModule
    {

        public YoyoProIdentityTestsModule(YoyoProIdentityEntityFrameworkCoreModule YoyoProIdentityEntityFrameworkCoreModule)
        {
            YoyoProIdentityEntityFrameworkCoreModule.SkipDbContextRegistration = true;

        }

        public override void PreInitialize()
        {
           
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(GetType().GetAssembly());
        }

        public override void PostInitialize()
        {
        }

        public override void Shutdown()
        {
        }
    }
}
