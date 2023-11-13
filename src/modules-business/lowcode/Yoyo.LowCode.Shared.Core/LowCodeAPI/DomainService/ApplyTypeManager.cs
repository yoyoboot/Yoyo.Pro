// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yoyo.Pro.Domain;

namespace Yoyo.LowCode.LowCodeAPI.DomainService
{
    public class ApplyTypeManager : BasicDomainService<ApplyType, Guid>, IApplyTypeManager
    {
        public ApplyTypeManager(IServiceProvider serviceProvider, string localizationSourceName = null) : base(serviceProvider, localizationSourceName)
        {
        }

        public async Task BatchDelete(List<Guid> input)
        {
            await EntityRepo.DeleteAsync(a => input.Contains(a.Id));
        }

        public async Task<ApplyType> CreateAsync(ApplyType entity)
        {
            entity.Id = await EntityRepo.InsertAndGetIdAsync(entity);

            return entity;
        }

        public async Task UpdateAsync(ApplyType entity)
        {
            await EntityRepo.UpdateAsync(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await EntityRepo.DeleteAsync(id);
        }
    }
}
