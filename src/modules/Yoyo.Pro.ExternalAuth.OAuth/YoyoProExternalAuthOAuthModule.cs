using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Yoyo.Pro
{
    [DependsOn(
       typeof(YoyoProExternalAuthModule)
       )]
    public class YoyoProExternalAuthOAuthModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(GetType().GetAssembly());
        }
    }
}
