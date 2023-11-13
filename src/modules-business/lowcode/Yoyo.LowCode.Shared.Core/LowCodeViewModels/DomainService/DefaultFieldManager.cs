// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yoyo.Pro.Domain;

namespace Yoyo.LowCode.LowCodeViewModels.DomainService
{
    public class DefaultFieldManager : BasicDomainService<LowCodeDefaultField, Guid>, IDefaultFieldManager
    {
        public DefaultFieldManager(IServiceProvider serviceProvider, string localizationSourceName = null) : base(serviceProvider, localizationSourceName)
        {
        }

        public async Task<LowCodeDefaultField> CreateAsync(LowCodeDefaultField defaultField)
        {
            defaultField.Id = await EntityRepo.InsertAndGetIdAsync(defaultField);

            return defaultField;
        }

        public async Task DeleteAsync(Guid id)
        {
            await EntityRepo.DeleteAsync(id);
        }

        public async Task UpdateAsync(LowCodeDefaultField entity)
        {
            await EntityRepo.UpdateAsync(entity);
        }

        public async Task BatchDelete(List<Guid> input)
        {
            await EntityRepo.DeleteAsync(a => input.Contains(a.Id));
        }
    }
}
