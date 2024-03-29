using Abp.Dependency;
using Abp.MultiTenancy;

using System.Collections.Generic;
using System.Text;
using Abp;
using Abp.BlobStoring;

namespace Yoyo.Pro.BlobStoring
{
    public class DefaultMinioBlobNameCalculator : IMinioBlobNameCalculator, ITransientDependency
    {
        protected ICurrentTenant CurrentTenant { get; }

        public DefaultMinioBlobNameCalculator(ICurrentTenant currentTenant)
        {
            CurrentTenant = currentTenant;
        }

        public virtual string Calculate(BlobProviderArgs args)
        {
            return !CurrentTenant.Id.HasValue()
                ? $"host/{args.BlobName}"
                : $"tenants/{CurrentTenant.Id}/{args.BlobName}";
        }
    }
}
