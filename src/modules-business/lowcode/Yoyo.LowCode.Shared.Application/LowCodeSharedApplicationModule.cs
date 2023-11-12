using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Yoyo.LowCode.Application.UserQueryModule.UserQueryDefinition.CustomDtoAutoMapper;
using Yoyo.LowCode.Application.UserQueryModule.UserQueryGroupDefinition.CustomDtoAutoMapper;
using Yoyo.LowCode.Authorization;
using Yoyo.LowCode.BaseStagingHistorys.Mapper;
using Yoyo.LowCode.ComField.Mapper;
using Yoyo.LowCode.CustomDtoAutoMapper;
using Yoyo.LowCode.DataDictionarys.Authorization;
using Yoyo.LowCode.DataDictionarys.Mapper;
using Yoyo.LowCode.LowCodeDefaultFields.Mapper;
using Yoyo.LowCode.LowCodeViewModels.Mapper;
using Yoyo.LowCode.SettingProviders;
using Yoyo.Pro;
using Yoyo.LowCode.LowCodeAPI.Mapper;

namespace Yoyo.LowCode
{
    [DependsOn(
        typeof(LowCodeCoreSharedModule),
        typeof(YoyoProIdentityApplicationModule)
    )]
    public class LowCodeSharedApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            // 配置Dto映射
            this.ConfigureAutoMapper();

            // 注册权限 AuthorizationProvider
            // 注册权限 AuthorizationProvider

            if (!LowCodeConfigs.Authorization.SkipAuthorization)
            {
                Configuration.Authorization.Providers.Add<DbLinkAuthorizationProvider>();

                Configuration.Authorization.Providers.Add<DynamicMenuAuthorizationProvider>();

                Configuration.Authorization.Providers.Add<CustomPageAuthorizationProvider>();

                Configuration.Authorization.Providers.Add<BaseDictionaryAuthorizationProvider>();
            }

            // 设置
            Configuration.Settings.Providers.Add<SystemTableSettingProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(this.GetType().GetAssembly());
        }

        public override void PostInitialize()
        {
        }

        private void ConfigureAutoMapper()
        {
            Configuration.Modules.AbpAutoMapper().Configurators.Add(configuration =>
            {
                // ....其他代码

                // 只需要复制这一段
                DbLinkDtoAutoMapper.CreateMappings(configuration);

                DatabaseDtoAutoMapper.CreateMappings(configuration);

                DynamicMenuDtoAutoMapper.CreateMappings(configuration);

                TemplateDataDtoAutoMapper.CreateMappings(configuration);

                DataDictionaryDtoAutoMapper.CreateMappings(configuration);

                ComFieldDtoAutoMapper.CreateMappings(configuration);

                UserQueryGroupDtoAutoMapper.CreateMappings(configuration);

                UserQueryDtoAutoMapper.CreateMappings(configuration);

                CustomPageDtoAutoMapper.CreateMappings(configuration);
                TemplateDataDtoAutoMapper.CreateMappings(configuration);

                BaseStagingHistoryAutoMapper.CreateMappings(configuration);

                LowCodeViewModelDtoAutoMapper.CreateMappings(configuration);

                ApplyDtoAutoMapper.CreateMappings(configuration);
                ApplyAPIDtoAutoMapper.CreateMappings(configuration);
                ApplyAuthenticationDtoAutoMapper.CreateMappings(configuration);
                ApplyHealthTestingDtoAutoMapper.CreateMappings(configuration);

                ApplyTypeDtoAutoMapper.CreateMappings(configuration);

                CommonTypeTableDtoAutoMapper.CreateMappings(configuration);

                AgentAPIDtoAutoMapper.CreateMappings(configuration);

                AppAuthDtoAutoMapper.CreateMappings(configuration);

                FlowControlDtoAutoMapper.CreateMappings(configuration);

                RenderDtoAutoMapper.CreateMappings(configuration);

                LowCodeDefaultFieldAutoMapper.CreateMappings(configuration);
                // ....其他代码
            });
        }
    }
}
