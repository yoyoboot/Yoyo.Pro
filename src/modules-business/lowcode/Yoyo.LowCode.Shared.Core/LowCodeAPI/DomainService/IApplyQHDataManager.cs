// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yoyo.Pro.Domain;

namespace Yoyo.LowCode.LowCodeAPI.DomainService
{
    public interface IApplyQHDataManager : IBasicDomainService<ApplyQHData, Guid>
    {
        /// <summary>
        ///     添加功能设计
        /// </summary>
        /// <param name="entity">功能设计实体</param>
        /// <returns></returns>
        Task<List<ApplyQHData>> CreateAsync(List<ApplyQHData> entity);

        /// <summary>
        ///     批量删除
        /// </summary>
        /// <param name="input">Id的集合</param>
        /// <returns></returns>
        Task BatchDelete(List<Guid> input);
    }
}
