// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yoyo.LowCode.StagingHistory.Dto;

namespace Yoyo.LowCode.BaseStagingHistorys
{
    public interface IBaseStagingHistoryAppService : IApplicationService
    {
        Task CreateStagingHistory(BaseStagingHistoryCreateDto input);

        Task<string> GetById(EntityDto<Guid> input);

        string JsonFormatConversion(DataFormatDto.BaseStagingHistoryDataFormatDto input);
    }
}
