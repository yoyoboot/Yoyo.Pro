// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yoyo.Pro.Domain;

namespace Yoyo.LowCode.LowCodeViewModels.DomainService
{
    public interface IDefaultFieldManager : IBasicDomainService<LowCodeDefaultField, Guid>
    {
        Task<LowCodeDefaultField> CreateAsync(LowCodeDefaultField lowCodeField);

        Task DeleteAsync(Guid id);

        Task UpdateAsync(LowCodeDefaultField entity);

        Task BatchDelete(List<Guid> input);
    }
}
