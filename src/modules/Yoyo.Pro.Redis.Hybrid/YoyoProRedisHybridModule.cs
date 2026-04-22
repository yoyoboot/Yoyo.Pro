using Abp;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Runtime.Caching.Redis;

namespace Yoyo.Pro
{
    [DependsOn(typeof(AbpRedisCacheModule))]
    public class YoyoProRedisHybridModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(YoyoProRedisHybridModule).GetAssembly());
        }
    }
}
