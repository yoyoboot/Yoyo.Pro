using Abp.Dependency;
using Abp.MultiTenancy;
using Abp;
using Abp.Runtime.Session;
using Abp.BlobStoring;

namespace Yoyo.Pro.BlobStoring
{
    public class DefaultAliyunBlobNameCalculator : IAliyunBlobNameCalculator, ITransientDependency
    {
        protected IAbpSession Session { get; }

        public DefaultAliyunBlobNameCalculator(IAbpSession session)
        {
            Session = session;
        }

        public virtual string Calculate(BlobProviderArgs args)
        {
            return !Session.TenantId.HasValue()
                ? $"host/{args.BlobName}"
                : $"tenants/{Session.TenantId}/{args.BlobName}";
        }
    }


}
