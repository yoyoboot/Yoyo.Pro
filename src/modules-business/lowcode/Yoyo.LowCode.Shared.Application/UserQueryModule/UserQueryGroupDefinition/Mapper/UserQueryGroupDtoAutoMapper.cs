// ReSharper disable once CheckNamespace
using AutoMapper;
using Yoyo.LowCode.UserQueryModule.UserQueryGroupDefinition;
using Yoyo.LowCode.UserQueryModule.UserQueryGroupDefinition.Dtos;
using Yoyo.LowCode.UserQueryModule.UserQueryRelationalDefinition;

namespace Yoyo.LowCode.Application.UserQueryModule.UserQueryGroupDefinition.CustomDtoAutoMapper
{
    /// <summary>
    /// 配置 BaseUserQueryGroup 的AutoMapper映射
    /// 前往 <see cref="LowCodeApplicationModule"/>的AbpAutoMapper配置方法下添加以下代码段
    /// UserQueryGroupDtoAutoMapperv.CreateMappings(configuration);
    /// </summary>
    public static class UserQueryGroupDtoAutoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<BaseUserQueryGroup, BaseUserQueryGroupListDto>().ReverseMap();
            configuration.CreateMap<BaseUserQueryGroup, BaseUserQueryGroupEditDto>().ReverseMap();
            configuration.CreateMap<BaseUserQueryGroupEntityDto, BaseUserQueryGroupEntity>()
                .ForMember(o => o.BaseUserQueryGroup, o => o.Ignore())
                .ForMember(o => o.BaseUserQuery, o => o.Ignore())
                .ReverseMap();
            configuration.CreateMap<BaseUserQueryGroupSubDto, BaseQueryGroupSub>()
                .ForMember(o => o.SubBaseUserQueryGroup, o => o.Ignore())
                .ForMember(o => o.BaseUserQueryGroup, o => o.Ignore())
                .ReverseMap();
        }
    }
}
