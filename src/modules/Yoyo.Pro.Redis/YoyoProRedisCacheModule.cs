using Abp;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Runtime.Caching.Redis;

namespace Yoyo.Pro
{
    [DependsOn(typeof(AbpKernelModule))]
    public class YoyoProRedisCacheModule : AbpModule
    {
        public override void PreInitialize()
        {
            IocManager.Register<AbpRedisCacheOptions>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(YoyoProRedisCacheModule).GetAssembly());
        }
    }
}
