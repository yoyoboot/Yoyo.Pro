// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using AutoMapper;
using Yoyo.LowCode.LowCodeAPI.Dtos.ApplyApiDto;
using Yoyo.LowCode.LowCodeAPI.Dtos.ApplyApiParameterDto;

namespace Yoyo.LowCode.LowCodeAPI.Mapper
{
    public class ApplyAPIDtoAutoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<ApplyAPI, ApplyAPIListDto>();
            configuration.CreateMap<ApplyAPIListDto, ApplyAPI>();

            configuration.CreateMap<ApplyAPI, ApplyAPIEditDto>();
            configuration.CreateMap<ApplyAPIEditDto, ApplyAPI>();

            ///API参数
            configuration.CreateMap<ApplyBodyData, ApplyBodyDataEditDto>();
            configuration.CreateMap<ApplyBodyDataEditDto, ApplyBodyData>();

            configuration.CreateMap<ApplyBodyData, ApplyBodyDataListDto>();
            configuration.CreateMap<ApplyBodyDataListDto, ApplyBodyData>();

            configuration.CreateMap<ApplyQHData, ApplyQHDataEditDto>();
            configuration.CreateMap<ApplyQHDataEditDto, ApplyQHData>();

            configuration.CreateMap<ApplyQHData, ApplyQHDataListDto>();
            configuration.CreateMap<ApplyQHDataListDto, ApplyQHData>();

            /*        configuration.CreateMap<SelectDictionaryValue, BaseDictionaryValue>().ReverseMap();*/
            //// custom codes

            //// custom codes end
        }
    }
}
