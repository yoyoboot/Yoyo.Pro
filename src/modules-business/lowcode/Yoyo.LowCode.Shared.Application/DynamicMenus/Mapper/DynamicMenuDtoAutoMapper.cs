// ReSharper disable once CheckNamespace
using AutoMapper;
using Yoyo.LowCode.DynamicMenus;
using Yoyo.LowCode.DynamicMenus.Dtos;

namespace Yoyo.LowCode.CustomDtoAutoMapper
{
    /// <summary>
    /// 配置DynamicMenu的AutoMapper映射
    /// 前往 <see cref="LowCodeApplicationModule"/>的AbpAutoMapper配置方法下添加以下代码段
    /// DynamicMenuDtoAutoMapper.CreateMappings(configuration);
    /// </summary>
    public static class DynamicMenuDtoAutoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<BaseDynamicMenu, LowCodeDynamicMenuListDto>();
            configuration.CreateMap<LowCodeDynamicMenuListDto, BaseDynamicMenu>();

            configuration.CreateMap<LowCodeDynamicMenuEditDto, BaseDynamicMenu>();
            configuration.CreateMap<BaseDynamicMenu, LowCodeDynamicMenuEditDto>();

            //// custom codes

            //// custom codes end
        }
    }
}
