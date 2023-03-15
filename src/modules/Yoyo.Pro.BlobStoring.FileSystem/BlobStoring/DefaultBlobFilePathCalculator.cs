using Abp.Dependency;
using Abp.MultiTenancy;

using System.Collections.Generic;
using System.IO;
using System.Text;
using Abp;

namespace Yoyo.Pro.BlobStoring
{
    public class DefaultBlobFilePathCalculator : IBlobFilePathCalculator, ITransientDependency
    {
        protected ICurrentTenant CurrentTenant { get; }

        public DefaultBlobFilePathCalculator(ICurrentTenant currentTenant)
        {
            CurrentTenant = currentTenant;
        }

        public virtual string Calculate(BlobProviderArgs args)
        {
            var fileSystemConfiguration = args.Configuration.GetFileSystemConfiguration();
            var blobPath = fileSystemConfiguration.BasePath;

            if (!CurrentTenant.Id.HasValue())
            {
                blobPath = Path.Combine(blobPath, "host");
            }
            else
            {
                blobPath = Path.Combine(blobPath, "tenants", CurrentTenant.Id);
            }

            if (fileSystemConfiguration.AppendContainerNameToBasePath)
            {
                blobPath = Path.Combine(blobPath, args.ContainerName);
            }

            blobPath = Path.Combine(blobPath, args.BlobName);

            return blobPath;
        }
    }


}
