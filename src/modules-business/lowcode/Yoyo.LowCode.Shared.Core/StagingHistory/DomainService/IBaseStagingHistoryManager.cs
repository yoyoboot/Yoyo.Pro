// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Yoyo.LowCode.StagingHistory.Dto;
using Yoyo.Pro.Domain;
using static Yoyo.LowCode.StagingHistory.Dto.DataFormatDto;

namespace Yoyo.LowCode.StagingHistory.DomainService
{
    public interface IBaseStagingHistoryManager : IBasicDomainService<BaseStagingHistory, Guid>
    {
        /// <summary>
        /// 添加暂存数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<BaseStagingHistory> CreateAsync(BaseStagingHistoryCreateDto input);

        /// <summary>
        /// 数据转换
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        string HandleFormatJson(BaseStagingHistoryDataFormatDto input);
    }
}
