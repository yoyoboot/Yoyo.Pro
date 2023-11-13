// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using AutoMapper;
using Yoyo.LowCode.LowCodeAPI.Dtos.CommonTypeTableDto;
using Yoyo.LowCode.LowCodeAPI.Dtos.Modeling;

namespace Yoyo.LowCode.LowCodeAPI.Mapper
{
    public class CommonTypeTableDtoAutoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CommonTypeTable, CommonTypeTableListDto>();
            configuration.CreateMap<CommonTypeTableListDto, CommonTypeTable>();

            configuration.CreateMap<CommonTypeTable, CommonTypeTableEditDto>();
            configuration.CreateMap<CommonTypeTableEditDto, CommonTypeTable>();

            configuration.CreateMap<AgentAPI, DataApiList>()
                .ForMember(dest => dest.ApiName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ApiPath, opt => opt.MapFrom(src => src.Path))
                .ForMember(dest => dest.ApiMethod, opt => opt.MapFrom(src => src.MethodName))
                .ForMember(dest => dest.LableName, opt => opt.MapFrom(src => src.LableName))
                .ForMember(dest => dest.Remarks, opt => opt.MapFrom(src => src.Remarks));

            /*        configuration.CreateMap<SelectDictionaryValue, BaseDictionaryValue>().ReverseMap();*/
            //// custom codes

            //// custom codes end
        }
    }
}
