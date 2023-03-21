using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Yoyo.Pro
{
    public class YoyoProAlipayModule : AbpModule
    {
        public YoyoProAlipayModule()
        {

        }

        public override void Initialize()
        {
            var thisAssembly = typeof(YoyoProAlipayModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);
        }

        public override void PostInitialize()
        {

        }

        public override void PreInitialize()
        {

        }
    }
}
