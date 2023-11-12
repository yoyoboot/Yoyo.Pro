// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yoyo.Pro.Domain;

namespace Yoyo.LowCode.LowCodeAPI.DomainService
{
    public interface IApplyAuthenticationManager : IBasicDomainService<ApplyAuthentication, Guid>
    {
        /// <summary>
        ///     添加功能设计
        /// </summary>
        /// <param name="entity">功能设计实体</param>
        /// <returns></returns>
        Task<ApplyAuthentication> CreateAsync(ApplyAuthentication entity);

        /// <summary>
        ///     修改功能设计
        /// </summary>
        /// <param name="entity">功能设计实体</param>
        /// <returns></returns>
        Task UpdateAsync(ApplyAuthentication entity);

        /// <summary>
        ///     删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(Guid id);

        /// <summary>
        ///     批量删除
        /// </summary>
        /// <param name="input">Id的集合</param>
        /// <returns></returns>
        Task BatchDelete(List<Guid> input);
    }
}
