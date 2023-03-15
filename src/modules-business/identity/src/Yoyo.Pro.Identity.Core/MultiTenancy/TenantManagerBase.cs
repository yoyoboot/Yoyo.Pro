using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Domain.Repositories;
using Abp.MultiTenancy;
using Yoyo.Pro.Editions;
using Yoyo.Pro.Users;

namespace Yoyo.Pro.MultiTenancy.Tenants
{
    /// <summary>
    /// Tenant manager.
    /// </summary>
    public abstract class TenantManagerBase : AbpTenantManager<Tenant, User>
    {
        public TenantManagerBase(
            IRepository<Tenant> tenantRepository,
            IRepository<TenantFeatureSetting> tenantFeatureRepository,
            AbpEditionManager editionManager,
            IAbpZeroFeatureValueStore featureValueStore
            )
            : base(
                  tenantRepository,
                  tenantFeatureRepository,
                  editionManager,
                  featureValueStore
                  )
        {
        }
    }
}
