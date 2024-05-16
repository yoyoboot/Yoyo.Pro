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
    public class AppAuthMappingManager : BasicDomainService<AppAuthMapping, Guid>, IAppAuthMappingManager
    {
        public AppAuthMappingManager(IServiceProvider serviceProvider, string localizationSourceName = null) : base(serviceProvider, localizationSourceName)
        {
        }

        public async Task BatchDelete(List<Guid> input)
        {
            var dbContext = EntityRepo.GetDbContext();
            await EntityRepo.DeleteAsync(a => input.Contains(a.Id));
        }

        public async Task<List<AppAuthMapping>> CreateAsync(List<AppAuthMapping> entity)
        {
            var dbContext = EntityRepo.GetDbContext();
            await dbContext.BulkInsertAsync(entity);
            return entity;
        }

        public async Task DeleteByEntity(List<AppAuthMapping> entity)
        {
            var dbContext = EntityRepo.GetDbContext();
            await dbContext.BulkDeleteAsync(entity);
        }
    }
}
