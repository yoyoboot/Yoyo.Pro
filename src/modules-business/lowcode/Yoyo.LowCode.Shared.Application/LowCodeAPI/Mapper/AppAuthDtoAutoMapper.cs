// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using AutoMapper;
using Yoyo.LowCode.LowCodeAPI.Dtos.AppAuthDto;

namespace Yoyo.LowCode.LowCodeAPI.Mapper
{
    public class AppAuthDtoAutoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<AppAuth, AppAuthEditDto>();
            configuration.CreateMap<AppAuthEditDto, AppAuth>();

            configuration.CreateMap<AppAuth, AppAuthListDto>();
            configuration.CreateMap<AppAuthListDto, AppAuth>();

            configuration.CreateMap<AppAuthMapping, AppAuthMappingEditDto>();
            configuration.CreateMap<AppAuthMappingEditDto, AppAuthMapping>();

            configuration.CreateMap<AppAuthMapping, AppAuthMappingListDto>();
            configuration.CreateMap<AppAuthMappingListDto, AppAuthMapping>();

            /*        configuration.CreateMap<SelectDictionaryValue, BaseDictionaryValue>().ReverseMap();*/
            //// custom codes

            //// custom codes end
        }
    }
}
