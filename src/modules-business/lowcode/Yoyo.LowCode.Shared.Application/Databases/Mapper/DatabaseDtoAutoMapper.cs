// ReSharper disable once CheckNamespace
using AutoMapper;
using Yoyo.LowCode.CustomPages.Dtos;
using Yoyo.LowCode.Databases.Dtos;
using Yoyo.LowCode.Databases.Entity;
using Yoyo.LowCode.Models;

namespace Yoyo.LowCode.CustomDtoAutoMapper
{
    /// <summary>
    /// 配置Database的AutoMapper映射
    /// 前往 <see cref="LowCodeApplicationModule"/>的AbpAutoMapper配置方法下添加以下代码段
    /// DbLinkDtoAutoMapper.CreateMappings(configuration);
    /// </summary>
    public static class DatabaseDtoAutoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<DbTableModel, DatabaseTableListOutput>();

            configuration.CreateMap<DbTableFieldModel, TableFieldsSelectorOutput>();

            configuration.CreateMap<TableInfo, DbTableModel>();

            configuration.CreateMap<TableFieldListItem, DbTableFieldModel>();

            configuration.CreateMap<BaseDbTableRelation, DbTableRelationEditDto>().ReverseMap();

            configuration.CreateMap<BaseDbTableRelation, DbTableRelationListDto>().ReverseMap();

            //// custom codes

            //// custom codes end
        }
    }
}
