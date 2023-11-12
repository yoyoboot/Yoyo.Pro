// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using AutoMapper;
using Yoyo.LowCode.LowCodeViewModels.Dtos;
using Yoyo.LowCode.Models;

namespace Yoyo.LowCode.LowCodeViewModels.Mapper
{
    public class LowCodeViewModelDtoAutoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<LowCodeModelRelation, LowCodeModelRelationListDto>().ReverseMap();

            configuration.CreateMap<LowCodeModelRelation, LowCodeModelRelationEditDto>().ReverseMap();

            configuration.CreateMap<LowCodeTableFieldsSelectorOutput, LowCodeField>().ReverseMap();

            configuration.CreateMap<LowCodeField, LowCodeFieldListDto>();

            configuration.CreateMap<LowCodeFieldListDto, LowCodeField>();

            configuration.CreateMap<LowCodeField, LowCodeFieldEditDto>().ReverseMap();

            configuration.CreateMap<DbTableModel, TableListOutput>()
                 .ForMember(x => x.TableName, u => u.MapFrom(u => u.Table))
                 .ForMember(x => x.TableDesc, u => u.MapFrom(u => u.TableName)).ReverseMap();

            configuration.CreateMap<LowCodeFieldListDto, DbTableFieldModel>()
                .ForMember(x => x.PrimaryKey, u => u.MapFrom(u => u.IsPrimaryKey == true ? 1 : 0))
                .ForMember(x => x.AllowNull, u => u.MapFrom(u => u.IsAllowNull == true ? 1 : 0))
                .ForMember(x => x.Field, u => u.MapFrom(u => u.FieldName))
                .ForMember(x => x.FieldName, u => u.MapFrom(u => u.FieldDesc));

            configuration.CreateMap<DbTableFieldModel, LowCodeFieldListDto>()
              .ForMember(x => x.IsPrimaryKey, u => u.MapFrom(u => u.PrimaryKey == 1 ? true : false))
              .ForMember(x => x.IsAllowNull, u => u.MapFrom(u => u.AllowNull == 1 ? true : false))
              .ForMember(x => x.FieldName, u => u.MapFrom(u => u.Field))
              .ForMember(x => x.FieldDesc, u => u.MapFrom(u => u.FieldName));

            configuration.CreateMap<LowCodeFieldEditDto, DbTableFieldModel>()
                .ForMember(x => x.PrimaryKey, u => u.MapFrom(u => u.IsPrimaryKey == true ? 1 : 0))
                .ForMember(x => x.AllowNull, u => u.MapFrom(u => u.IsAllowNull == true ? 1 : 0))
                .ForMember(x => x.Field, u => u.MapFrom(u => u.FieldName))
                .ForMember(x => x.FieldName, u => u.MapFrom(u => u.FieldDesc)).ReverseMap();

            configuration.CreateMap<LowCodeField, DbTableFieldModel>()
                .ForMember(x => x.PrimaryKey, u => u.MapFrom(u => u.IsPrimaryKey == true ? 1 : 0))
                .ForMember(x => x.AllowNull, u => u.MapFrom(u => u.IsAllowNull == true ? 1 : 0))
                .ForMember(x => x.Field, u => u.MapFrom(u => u.FieldName))
                .ForMember(x => x.FieldName, u => u.MapFrom(u => u.FieldDesc));

            configuration.CreateMap<DbTableFieldModel, LowCodeField>()
                .ForMember(x => x.IsPrimaryKey, u => u.MapFrom(u => u.PrimaryKey == 1 ? true : false))
                .ForMember(x => x.IsAllowNull, u => u.MapFrom(u => u.AllowNull == 1 ? true : false))
                .ForMember(x => x.FieldName, u => u.MapFrom(u => u.Field))
                .ForMember(x => x.FieldDesc, u => u.MapFrom(u => u.FieldName));

            configuration.CreateMap<NewTableInfo, DbTableModel>()
                    .ForMember(x => x.Table, u => u.MapFrom(u => u.NewTableName))
                    .ForMember(x => x.TableName, u => u.MapFrom(u => u.TableDesc)).ReverseMap();

            configuration.CreateMap<DbTableModel, TableListSelectOutput>()
                 .ForMember(x => x.TableName, u => u.MapFrom(u => u.Table))
                 .ForMember(x => x.TableDesc, u => u.MapFrom(u => u.TableName))
                 .ForMember(x => x.PrimaryKey, u => u.MapFrom(u => u.PrimaryKey));
        }
    }
}
