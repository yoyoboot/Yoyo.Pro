// ReSharper disable once CheckNamespace
using AutoMapper;
using Yoyo.LowCode.AutomaticTables.Dtos;
using Yoyo.LowCode.CustomPages;
using Yoyo.LowCode.CustomPages.Dtos;
using Yoyo.LowCode.Databases.Entity;

namespace Yoyo.LowCode.CustomDtoAutoMapper
{
    /// <summary>
    /// 配置BaseCustomPage的AutoMapper映射
    /// 前往 <see cref="LowCodeApplicationModule"/>的AbpAutoMapper配置方法下添加以下代码段
    /// CustomPageDtoAutoMapper.CreateMappings(configuration);
    /// </summary>
    public static class CustomPageDtoAutoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<BaseCustomPage, CustomPageListDto>();
            configuration.CreateMap<CustomPageListDto, BaseCustomPage>();

            configuration.CreateMap<CustomPageEditDto, BaseCustomPage>();
            configuration.CreateMap<BaseCustomPage, CustomPageEditDto>();

            configuration.CreateMap<BaseDbTableRelationEditDto, BaseDbTableRelation>().ReverseMap();

            configuration.CreateMap<TrasferimentoEditDto, TrasferimentoDto>().ReverseMap();
            //// custom codes

            //// custom codes end
        }
    }
}
