// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.EntityFrameworkCore.Repositories;
using EFCore.BulkExtensions;
using Yoyo.Pro.Domain;

namespace Yoyo.LowCode.LowCodeAPI.DomainService
{
    public class ApplyAPIManager : BasicDomainService<ApplyAPI, Guid>, IApplyAPIManager
    {
        public ApplyAPIManager(IServiceProvider serviceProvider, string localizationSourceName = null) : base(serviceProvider, localizationSourceName)
        {
        }

        public async Task BatchDelete(List<Guid> input)
        {
            await EntityRepo.DeleteAsync(a => input.Contains(a.Id));
        }

        public async Task<ApplyAPI> CreateAsync(ApplyAPI entity)
        {
            entity.Id = await EntityRepo.InsertAndGetIdAsync(entity);

            return entity;
        }

        public async Task UpdateAsync(ApplyAPI entity)
        {
            await EntityRepo.UpdateAsync(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await EntityRepo.DeleteAsync(id);
        }

        public async Task<List<ApplyAPI>> BatchCreateAsync(List<ApplyAPI> entity)
        {
            var dbContext = EntityRepo.GetDbContext();
            await dbContext.BulkInsertAsync(entity);
            return entity;
        }
    }
}
