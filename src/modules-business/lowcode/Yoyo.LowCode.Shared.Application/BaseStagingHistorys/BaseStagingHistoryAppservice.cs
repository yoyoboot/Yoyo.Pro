// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using Yoyo.LowCode.BaseStagingHistorys.Dtos;
using Yoyo.LowCode.StagingHistory.DomainService;
using Yoyo.LowCode.StagingHistory.Dto;

namespace Yoyo.LowCode.BaseStagingHistorys
{
    /// <summary>
    ///  暂存功能
    /// </summary>
    public class BaseStagingHisToryAppService : LowCodeSharedAppServiceBase, IBaseStagingHistoryAppService
    {
        private readonly IBaseStagingHistoryManager _stagingHistoryManager;

        public BaseStagingHisToryAppService(IBaseStagingHistoryManager stagingHistoryManager)
        {
            _stagingHistoryManager = stagingHistoryManager;
        }

        public async Task CreateStagingHistory(BaseStagingHistoryCreateDto input)
        {
            await _stagingHistoryManager.CreateAsync(input);
        }

        public async Task<string> GetById(EntityDto<Guid> input)
        {
            var entity = await _stagingHistoryManager.QueryAsNoTracking
                .Where(x => x.Key == input.Id)
                .OrderByDescending(x => x.CreationTime)
                .FirstOrDefaultAsync();

            var dto = ObjectMapper.Map<BaseStagingHistoryListDto>(entity);
            return dto?.StagingJson;
        }

        /// <summary>
        /// json格式转换
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string JsonFormatConversion(DataFormatDto.BaseStagingHistoryDataFormatDto input)
        {
            return _stagingHistoryManager.HandleFormatJson(input);
        }
    }
}
