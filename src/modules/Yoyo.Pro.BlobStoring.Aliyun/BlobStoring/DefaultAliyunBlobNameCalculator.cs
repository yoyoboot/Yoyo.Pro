using Abp.Dependency;
using Abp.MultiTenancy;
using Abp;

namespace Yoyo.Pro.BlobStoring
{
    public class DefaultAliyunBlobNameCalculator : IAliyunBlobNameCalculator, ITransientDependency
    {
        protected ICurrentTenant CurrentTenant { get; }

        public DefaultAliyunBlobNameCalculator(ICurrentTenant currentTenant)
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
