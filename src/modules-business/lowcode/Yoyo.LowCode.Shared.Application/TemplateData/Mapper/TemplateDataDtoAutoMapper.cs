// ReSharper disable once CheckNamespace

using AutoMapper;
using Yoyo.LowCode.TemplateData.Dtos;
using Yoyo.LowCode.TemplateData.Entity;

namespace Yoyo.LowCode.CustomDtoAutoMapper
{
    /// <summary>
    /// 配置BaseCustomPage的AutoMapper映射
    /// 前往 <see cref="LowCodeApplicationModule"/>的AbpAutoMapper配置方法下添加以下代码段
    /// CustomPageDtoAutoMapper.CreateMappings(configuration);
    /// </summary>
    public static class TemplateDataDtoAutoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<BaseTemplateData, BaseTemplateDataListDto>().ReverseMap();
            //// custom codes

            //// custom codes end
        }
    }
}
