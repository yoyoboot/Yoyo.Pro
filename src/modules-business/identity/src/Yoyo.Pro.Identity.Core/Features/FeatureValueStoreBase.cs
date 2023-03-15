// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Features;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.MultiTenancy;
using Abp.Runtime.Caching;
using Yoyo.Pro.MultiTenancy;
using Yoyo.Pro.Users;

namespace Yoyo.Pro.Features
{
    public abstract class FeatureValueStoreBase : AbpFeatureValueStore<Tenant, User>
    {
        public FeatureValueStoreBase(
            ICacheManager cacheManager,
            IRepository<TenantFeatureSetting> tenantFeatureRepository,
            IRepository<Tenant> tenantRepository,
            IRepository<EditionFeatureSetting> editionFeatureRepository,
            IFeatureManager featureManager,
            IUnitOfWorkManager unitOfWorkManager)
            : base(
                  cacheManager,
                  tenantFeatureRepository,
                  tenantRepository,
                  editionFeatureRepository,
                  featureManager,
                  unitOfWorkManager)
        {
        }
    }
}
