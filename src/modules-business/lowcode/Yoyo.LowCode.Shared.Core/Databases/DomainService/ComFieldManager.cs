// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Yoyo.LowCode.Databases.Entity;
using Yoyo.Pro.Domain;

namespace Yoyo.LowCode.Databases.DomainService
{
    public class ComFieldManager : BasicDomainService<BaseComFields, Guid>, IComFieldManager
    {
        public ComFieldManager(IServiceProvider serviceProvider, string localizationSourceName = null) : base(serviceProvider, localizationSourceName)
        {
        }

        public async Task<BaseComFields> CreateAsync(BaseComFields entity)
        {
            entity.Id = await EntityRepo.InsertAndGetIdAsync(entity);

            return entity;
        }

        public async Task UpdateAsync(BaseComFields entity)
        {
            await EntityRepo.UpdateAsync(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await EntityRepo.DeleteAsync(id);
        }
    }
}
