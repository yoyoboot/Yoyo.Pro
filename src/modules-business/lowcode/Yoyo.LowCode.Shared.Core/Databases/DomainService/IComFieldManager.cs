// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Yoyo.LowCode.Databases.Entity;
using Yoyo.Pro.Domain;

namespace Yoyo.LowCode.Databases.DomainService
{
    public interface IComFieldManager : IBasicDomainService<BaseComFields, Guid>
    {
        Task<BaseComFields> CreateAsync(BaseComFields entity);

        /// <summary>
        ///     删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(Guid id);

        Task UpdateAsync(BaseComFields entity);
    }
}
