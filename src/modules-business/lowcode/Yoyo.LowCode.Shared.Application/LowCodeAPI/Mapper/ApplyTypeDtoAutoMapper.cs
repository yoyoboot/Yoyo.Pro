// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using AutoMapper;
using Yoyo.LowCode.LowCodeAPI.Dtos.ApplyTypeDto;

namespace Yoyo.LowCode.LowCodeAPI.Mapper
{
    public class ApplyTypeDtoAutoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<ApplyType, ApplyTypeListDto>();
            configuration.CreateMap<ApplyTypeListDto, ApplyType>();

            configuration.CreateMap<ApplyType, ApplyTypeEditDto>();
            configuration.CreateMap<ApplyTypeEditDto, ApplyType>();

            /*        configuration.CreateMap<SelectDictionaryValue, BaseDictionaryValue>().ReverseMap();*/
            //// custom codes

            //// custom codes end
        }
    }
}
