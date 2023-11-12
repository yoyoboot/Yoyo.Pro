// ReSharper disable once CheckNamespace
using AutoMapper;
using Yoyo.LowCode.Databases.Entity;
using Yoyo.LowCode.DbLinks.Dtos;

namespace Yoyo.LowCode.CustomDtoAutoMapper
{
    /// <summary>
    /// 配置BaseDbLink的AutoMapper映射
    /// 前往 <see cref="LowCodeApplicationModule"/>的AbpAutoMapper配置方法下添加以下代码段
    /// DbLinkDtoAutoMapper.CreateMappings(configuration);
    /// </summary>
    public static class DbLinkDtoAutoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<BaseDbLink, DbLinkListDto>();
            configuration.CreateMap<DbLinkListDto, BaseDbLink>();

            configuration.CreateMap<DbLinkEditDto, BaseDbLink>();
            configuration.CreateMap<BaseDbLink, DbLinkEditDto>();

            configuration.CreateMap<BaseDbLink, DbLinkSelectorOutput>();

            //// custom codes

            //// custom codes end
        }
    }
}
