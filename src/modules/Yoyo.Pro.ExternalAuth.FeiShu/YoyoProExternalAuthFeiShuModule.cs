using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Yoyo.Pro
{
    [DependsOn(
       typeof(YoyoProExternalAuthOAuthModule)
       )]
    public class YoyoProExternalAuthFeiShuModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(GetType().GetAssembly());
        }
    }
}