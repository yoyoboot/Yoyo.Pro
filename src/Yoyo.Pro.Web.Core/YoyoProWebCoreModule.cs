using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Yoyo.Pro
{
    [DependsOn(
        typeof(YoyoProAspNetCoreModule),
        typeof(YoyoProApplicationModule),
        typeof(YoyoProEntityFrameworkCoreModule),
        typeof(AbpAspNetCoreModule)
        )]
    public class YoyoProWebCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            // 注册动态webapi
            Configuration.Modules.AbpAspNetCore()
                .CreateControllersForAppServices(
                    typeof(YoyoProApplicationModule).GetAssembly()
                );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(YoyoProWebCoreModule).GetAssembly());

            // IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
        // 添加注释
        public override void PostInitialize()
        {
        }
    }
}
