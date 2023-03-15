
using JetBrains.Annotations;

using System.Threading;

namespace Yoyo.Pro.BlobStoring
{
    public class BlobProviderDeleteArgs : BlobProviderArgs
    {
        public BlobProviderDeleteArgs(
            [NotNull] string containerName,
            [NotNull] BlobContainerConfiguration configuration,
            [NotNull] string blobName,
            CancellationToken cancellationToken = default)
            : base(
                containerName,
                configuration,
                blobName,
                cancellationToken)
        {
        }
    }

}
