using Abp.Modules;
using Abp.Dependency;
using Abp.Reflection.Extensions;
using Abp.Zero.Configuration;
using Yoyo.Pro;

namespace Yoyo.Pro
{
    [DependsOn(typeof(YoyoProIdentityCoreModule),
        typeof(YoyoProEntityFrameworkCoreModule))]
    public class YoyoProIdentityEntityFrameworkCoreModule : AbpModule
    {
        /* 在单元测试的时候使用跳过DbContext注册，以便使用EF Core的内存数据库 */
        public bool SkipDbContextRegistration { get; set; }

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
