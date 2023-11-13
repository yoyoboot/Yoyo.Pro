// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using AutoMapper;
using Yoyo.LowCode.LowCodeAPI.Dtos.FlowControlDto;

namespace Yoyo.LowCode.LowCodeAPI.Mapper
{
    public class FlowControlDtoAutoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<FlowControl, FlowControlEditDto>();
            configuration.CreateMap<FlowControlEditDto, FlowControl>();

            configuration.CreateMap<FlowControl, FlowControlListDto>();
            configuration.CreateMap<FlowControlListDto, FlowControl>();

            configuration.CreateMap<FlowControlMapping, FlowControlMappingEditDto>();
            configuration.CreateMap<FlowControlMappingEditDto, FlowControlMapping>();

            configuration.CreateMap<FlowControlMapping, FlowControlMappingListDto>();
            configuration.CreateMap<FlowControlMappingListDto, FlowControlMapping>();

            /*        configuration.CreateMap<SelectDictionaryValue, BaseDictionaryValue>().ReverseMap();*/
            //// custom codes

            //// custom codes end
        }
    }
}
