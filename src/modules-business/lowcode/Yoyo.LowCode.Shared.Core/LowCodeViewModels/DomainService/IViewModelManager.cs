// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Yoyo.Pro.Domain;

namespace Yoyo.LowCode.LowCodeViewModels.DomainService

{
    public interface IViewModelManager : IBasicDomainService<LowCodeModel, Guid>
    {
        Task DeleteAsync(Guid id);

        Task UpdateAsync(LowCodeModel entity);

        Task<LowCodeModel> CreateAsync(LowCodeModel entity);

        Task BulkDeleteAsync(string tableName);

        Task MaintenanceTableInfo(string tableName);
    }
}
