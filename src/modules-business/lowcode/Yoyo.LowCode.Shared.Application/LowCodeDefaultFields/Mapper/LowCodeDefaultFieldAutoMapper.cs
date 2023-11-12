// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using AutoMapper;
using Yoyo.LowCode.LowCodeDefaultFields.Dtos;
using Yoyo.LowCode.LowCodeViewModels;

namespace Yoyo.LowCode.LowCodeDefaultFields.Mapper
{
    public class LowCodeDefaultFieldAutoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<LowCodeDefaultField, LowCodeDefaultFieldListDto>();
            configuration.CreateMap<LowCodeDefaultFieldListDto, LowCodeDefaultField>();

            configuration.CreateMap<LowCodeDefaultFieldEditDto, LowCodeDefaultField>();
            configuration.CreateMap<LowCodeDefaultField, LowCodeDefaultFieldEditDto>();

            //// custom codes

            //// custom codes end
        }
    }
}
