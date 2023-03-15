using Abp.Domain.Uow;
using Abp.Runtime.Session;

using JetBrains.Annotations;

using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Yoyo.Pro.BlobStoring
{
    public interface IBlobProviderSelector
    {
        [NotNull]
        IBlobProvider Get([NotNull] string containerName);
    }
}
