using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Yoyo.LowCode
{
    [DependsOn(
        typeof(LowCodeCoreSharedModule)
    )]
    public class LowCodeSharedEntityFrameworkCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
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
