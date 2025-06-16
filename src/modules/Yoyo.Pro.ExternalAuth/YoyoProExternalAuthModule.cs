using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Yoyo.Pro
{
    [DependsOn(
       typeof(YoyoProModule)
       )]
    public class YoyoProExternalAuthModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(GetType().GetAssembly());
        }
    }
}
