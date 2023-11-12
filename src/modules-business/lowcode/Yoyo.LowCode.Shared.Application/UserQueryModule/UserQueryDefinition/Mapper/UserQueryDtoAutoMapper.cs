// ReSharper disable once CheckNamespace
using AutoMapper;
using Yoyo.LowCode.UserQueryModule.UserQueryDefinition;
using Yoyo.LowCode.UserQueryModule.UserQueryDefinition.Dtos;

namespace Yoyo.LowCode.Application.UserQueryModule.UserQueryDefinition.CustomDtoAutoMapper
{
    /// <summary>
    /// 配置 BaseUserQuery 的AutoMapper映射
    /// 前往 <see cref="LowCodeApplicationModule"/>的AbpAutoMapper配置方法下添加以下代码段
    /// UserQueryDtoAutoMapper.CreateMappings(configuration);
    /// </summary>
    public static class UserQueryDtoAutoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<BaseUserQuery, BaseUserQueryListDto>().ReverseMap();
            configuration.CreateMap<BaseUserQuery, BaseUserQueryEditDto>()
                .ForMember(o => o.QueryParamters, opt => opt.MapFrom(x => x.QueryParamters))
                .ReverseMap();
        }
    }
}
