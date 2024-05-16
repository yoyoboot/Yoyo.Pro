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
    public class ApplyQHDataManager : BasicDomainService<ApplyQHData, Guid>, IApplyQHDataManager
    {
        public ApplyQHDataManager(IServiceProvider serviceProvider, string localizationSourceName = null) : base(serviceProvider, localizationSourceName)
        {
        }

        public async Task BatchDelete(List<Guid> input)
        {
            //await EntityRepo.DeleteAsync(a => input.Contains(a.Id));
            List<ApplyQHData> qHDatas = new List<ApplyQHData>();
            foreach (var data in input)
            {
                qHDatas.Add(new ApplyQHData
                {
                    Id = data
                });
            }
            var dbContext = EntityRepo.GetDbContext();
            await dbContext.BulkDeleteAsync(qHDatas);
        }

        public async Task<List<ApplyQHData>> CreateAsync(List<ApplyQHData> entity)
        {
            var dbContext = EntityRepo.GetDbContext();
            await dbContext.BulkInsertAsync(entity);
            return entity;
        }
    }
}
