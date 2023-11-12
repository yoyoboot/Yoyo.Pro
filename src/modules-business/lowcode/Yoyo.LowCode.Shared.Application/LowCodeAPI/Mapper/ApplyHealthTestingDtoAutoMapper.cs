// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using AutoMapper;
using Yoyo.LowCode.LowCodeAPI.Dtos.ApplyHealthTestingDto;

namespace Yoyo.LowCode.LowCodeAPI.Mapper
{
    public class ApplyHealthTestingDtoAutoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<ApplyHealthTesting, ApplyHealthTestingListDto>();
            configuration.CreateMap<ApplyHealthTestingListDto, ApplyHealthTesting>();

            configuration.CreateMap<ApplyHealthTesting, ApplyHealthTestingEditDto>();
            configuration.CreateMap<ApplyHealthTestingEditDto, ApplyHealthTesting>();

            /*        configuration.CreateMap<SelectDictionaryValue, BaseDictionaryValue>().ReverseMap();*/
            //// custom codes

            //// custom codes end
        }
    }
}
