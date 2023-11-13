// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yoyo.Pro.Domain;

namespace Yoyo.LowCode.LowCodeAPI.DomainService
{
    public class CommonTypeTableManager : BasicDomainService<CommonTypeTable, Guid>, ICommonTypeTableManager
    {
        public CommonTypeTableManager(IServiceProvider serviceProvider, string localizationSourceName = null) : base(serviceProvider, localizationSourceName)
        {
        }

        public async Task BatchDelete(List<Guid> input)
        {
            await EntityRepo.DeleteAsync(a => input.Contains(a.Id));
        }

        public async Task<CommonTypeTable> CreateAsync(CommonTypeTable entity)
        {
            entity.Id = await EntityRepo.InsertAndGetIdAsync(entity);

            return entity;
        }

        public async Task DeleteAsync(Guid id)
        {
            await EntityRepo.DeleteAsync(id);
        }

        public async Task UpdateAsync(CommonTypeTable entity)
        {
            await EntityRepo.UpdateAsync(entity);
        }
    }
}
