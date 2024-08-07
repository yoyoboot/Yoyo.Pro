using Abp.Dependency;
using Abp.BlobStoring;
using Abp.Runtime.Session;
using Abp;

namespace Yoyo.Pro.BlobStoring
{
    public class DefaultMinioBlobNameCalculator : IMinioBlobNameCalculator, ITransientDependency
    {
        protected IAbpSession AbpSession { get; }

        public DefaultMinioBlobNameCalculator(IAbpSession session)
        {
            AbpSession = session;
        }

        public virtual string Calculate(BlobProviderArgs args)
        {
            return !AbpSession.TenantId.HasValue()
                ? $"host/{args.BlobName}"
                : $"tenants/{AbpSession.TenantId}/{args.BlobName}";
        }
    }
}
