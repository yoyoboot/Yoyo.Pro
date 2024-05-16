// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using AutoMapper;
using Yoyo.LowCode.LowCodeAPI.Dtos.ApplyAuthenticationDto;

namespace Yoyo.LowCode.LowCodeAPI.Mapper
{
    public class ApplyAuthenticationDtoAutoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<ApplyAuthentication, ApplyAuthenticationListDto>();
            configuration.CreateMap<ApplyAuthenticationListDto, ApplyAuthentication>();

            configuration.CreateMap<ApplyAuthentication, ApplyAuthenticationEditDto>();
            configuration.CreateMap<ApplyAuthenticationEditDto, ApplyAuthentication>();

            /*        configuration.CreateMap<SelectDictionaryValue, BaseDictionaryValue>().ReverseMap();*/
            //// custom codes

            //// custom codes end
        }
    }
}
