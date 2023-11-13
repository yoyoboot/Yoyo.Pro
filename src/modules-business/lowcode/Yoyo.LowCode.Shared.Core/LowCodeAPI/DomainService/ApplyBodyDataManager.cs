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
    public class ApplyBodyDataManager : BasicDomainService<ApplyBodyData, Guid>, IApplyBodyDataManager
    {
        public ApplyBodyDataManager(IServiceProvider serviceProvider, string localizationSourceName = null) : base(serviceProvider, localizationSourceName)
        {
        }

        public async Task BatchDelete(List<Guid> input)
        {
            //await EntityRepo.DeleteAsync(a => input.Contains(a.Id));
            List<ApplyBodyData> bodyDatas = new List<ApplyBodyData>();
            foreach (var data in input)
            {
                bodyDatas.Add(new ApplyBodyData
                {
                    Id = data
                });
            }
            var dbContext = EntityRepo.GetDbContext();
            await dbContext.BulkDeleteAsync(bodyDatas);
        }

        public async Task<List<ApplyBodyData>> CreateAsync(List<ApplyBodyData> entity)
        {
            var dbContext = EntityRepo.GetDbContext();
            await dbContext.BulkInsertAsync(entity);
            return entity;
        }

        public async Task DeleteByEntity(List<ApplyBodyData> entity)
        {
            var dbContext = EntityRepo.GetDbContext();
            await dbContext.BulkDeleteAsync(entity);
        }
    }
}
