// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using AutoMapper;
using Yoyo.LowCode.BaseStagingHistorys.Dtos;
using Yoyo.LowCode.StagingHistory;

namespace Yoyo.LowCode.BaseStagingHistorys.Mapper
{
    public class BaseStagingHistoryAutoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<BaseStagingHistory, BaseStagingHistoryListDto>().ReverseMap();
        }
    }
}
