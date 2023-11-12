// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using AutoMapper;
using Yoyo.LowCode.LowCodeAPI.Dtos.AgentAPIDto;
using Yoyo.LowCode.LowCodeAPI.Dtos.APIRequestRecordDto;
using Yoyo.LowCode.LowCodeAPI.Dtos.ApplyApiParameterDto;

namespace Yoyo.LowCode.LowCodeAPI.Mapper
{
    public class AgentAPIDtoAutoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<AgentAPI, AgentAPIEditDto>();
            configuration.CreateMap<AgentAPIEditDto, AgentAPI>();

            configuration.CreateMap<AgentAPI, AgentAPIListDto>();
            configuration.CreateMap<AgentAPIListDto, AgentAPI>();

            configuration.CreateMap<AgentAPI, AgentAPIListAllDto>();
            configuration.CreateMap<AgentAPIListAllDto, AgentAPI>();

            configuration.CreateMap<ParameterMapping, ParameterMappingEditDto>();
            configuration.CreateMap<ParameterMappingEditDto, ParameterMapping>();

            configuration.CreateMap<ParameterMapping, ParameterMappingListDto>();
            configuration.CreateMap<ParameterMappingListDto, ParameterMapping>();

            configuration.CreateMap<APIRequestRecord, APIRequestRecordEditDto>();
            configuration.CreateMap<APIRequestRecordEditDto, APIRequestRecord>();

            configuration.CreateMap<APIRequestRecord, APIRequestRecordListDto>();
            configuration.CreateMap<APIRequestRecordListDto, APIRequestRecord>();

            /*        configuration.CreateMap<SelectDictionaryValue, BaseDictionaryValue>().ReverseMap();*/
            //// custom codes

            //// custom codes end
        }
    }
}
