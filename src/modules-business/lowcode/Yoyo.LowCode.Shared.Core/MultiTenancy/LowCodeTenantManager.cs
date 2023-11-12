using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Domain.Repositories;
using Abp.MultiTenancy;
using Yoyo.Pro.MultiTenancy;
using Yoyo.Pro.MultiTenancy.Tenants;

namespace Yoyo.LowCode.MultiTenancy
{
    /// <summary>
    /// Tenant manager.
    /// </summary>
    public class LowCodeTenantManager : TenantManagerBase
    {
        public LowCodeTenantManager(IRepository<Tenant> tenantRepository,
            IRepository<TenantFeatureSetting> tenantFeatureRepository, AbpEditionManager editionManager,
            IAbpZeroFeatureValueStore featureValueStore) : base(tenantRepository, tenantFeatureRepository,
            editionManager, featureValueStore)
        {
        }
    }
}
