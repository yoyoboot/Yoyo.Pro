using Abp.BlobStoring;

namespace Yoyo.Pro.BlobStoring
{
    public interface IAliyunBlobNameCalculator
    {
        string Calculate(BlobProviderArgs args);
    }


}
