// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yoyo.LowCode.Databases.Entity;
using Yoyo.Pro.Domain;

namespace Yoyo.LowCode.Databases.DomainService
{
    public class DbTableRelationManager : BasicDomainService<BaseDbTableRelation, Guid>, IDbTableRelationManager
    {
        public DbTableRelationManager(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<List<BaseDbTableRelation>> BlukCreateAsync(List<BaseDbTableRelation> entityList)
        {
            foreach (var item in entityList)
            {
                item.Id = await EntityRepo.InsertAndGetIdAsync(item);
            }
            return entityList;
        }

        public async Task<BaseDbTableRelation> CreateAsync(BaseDbTableRelation entity)
        {
            entity.Id = await EntityRepo.InsertAndGetIdAsync(entity);

            return entity;
        }

        public async Task DeleteAsync(Guid id)
        {
            await EntityRepo.DeleteAsync(id);
        }

        public async Task UpdateAsync(BaseDbTableRelation entity)
        {
            await EntityRepo.UpdateAsync(entity);
        }
    }
}
