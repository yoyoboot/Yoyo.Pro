// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using AutoMapper;
using Yoyo.LowCode.LowCodeAPI.Dtos.ApplyDto;

namespace Yoyo.LowCode.LowCodeAPI.Mapper
{
    public class ApplyDtoAutoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Apply, ApplyListDto>();
            configuration.CreateMap<ApplyListDto, Apply>();

            configuration.CreateMap<Apply, ApplyEditDto>();
            configuration.CreateMap<ApplyEditDto, Apply>();

            /*        configuration.CreateMap<SelectDictionaryValue, BaseDictionaryValue>().ReverseMap();*/
            //// custom codes

            //// custom codes end
        }
    }
}
