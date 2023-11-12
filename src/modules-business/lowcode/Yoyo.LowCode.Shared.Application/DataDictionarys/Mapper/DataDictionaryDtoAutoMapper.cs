// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using AutoMapper;
using Yoyo.LowCode.DataDictionarys.Dtos;

namespace Yoyo.LowCode.DataDictionarys.Mapper
{
    public class DataDictionaryDtoAutoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<BaseDictionaryType, LowCodeDataDictionaryListDto>();
            configuration.CreateMap<LowCodeDataDictionaryListDto, BaseDictionaryType>();

            configuration.CreateMap<LowCodeDataDictionaryEditDto, BaseDictionaryType>();
            configuration.CreateMap<BaseDictionaryType, LowCodeDataDictionaryEditDto>();

            configuration.CreateMap<LowCodeDictionaryValueListDto, BaseDictionaryValue>().ReverseMap();
            configuration.CreateMap<DictionaryValueEditDto, BaseDictionaryValue>().ReverseMap();

            configuration.CreateMap<BaseDictionaryValueEditDto, BaseDictionaryValue>().ReverseMap();

            configuration.CreateMap<SelectDictionaryValue, BaseDictionaryValue>()
                .ForMember(res => res.Id, src => src.MapFrom(x => x.Value))
                .ForMember(res => res.Name, src => src.MapFrom(x => x.Label))
                ;

            configuration.CreateMap<BaseDictionaryValue, SelectDictionaryValue>()
                .ForMember(res => res.Value, src => src.MapFrom(x => x.Id))
                .ForMember(res => res.Label, src => src.MapFrom(x => x.Name))
                ;
            /*        configuration.CreateMap<SelectDictionaryValue, BaseDictionaryValue>().ReverseMap();*/
            //// custom codes

            //// custom codes end
        }
    }
}
