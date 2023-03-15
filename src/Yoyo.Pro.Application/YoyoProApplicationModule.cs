using System.Reflection;
using Abp.AutoMapper;
using Abp.Modules;
using Yoyo.Pro.Modules.DynamicView.Mapper;

namespace Yoyo.Pro
{
    [DependsOn(
        typeof(YoyoProCoreModule),
        typeof(AbpAutoMapperModule))]
    public class YoyoProApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Modules.AbpAutoMapper().Configurators.Add(configuration =>
            {
                CustomerAppMapper.CreateMappings(configuration);
                // 动态页面信息
                DynamicViewMapper.CreateMappings(configuration);
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        public override void PostInitialize()
        {
        }
    }
}
