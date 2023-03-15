using Abp.Modules;
using Abp.Reflection.Extensions;
using Yoyo.Pro;

namespace Yoyo.Pro
{
    [DependsOn(typeof(YoyoProApplicationModule), typeof(YoyoProIdentityCoreModule))]
    public class YoyoProIdentityApplicationModule : AbpModule
    {
        public override void PreInitialize()
        { 

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
