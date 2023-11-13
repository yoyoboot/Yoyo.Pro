using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Yoyo.LowCode
{
    [DependsOn(
        typeof(LowCodeCoreSharedModule)
    )]
    public class LowCodeSharedWebCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(LowCodeSharedWebCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
        }
    }
}
