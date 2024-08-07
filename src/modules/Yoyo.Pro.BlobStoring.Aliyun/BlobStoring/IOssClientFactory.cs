
using Aliyun.OSS;

namespace Yoyo.Pro.BlobStoring
{
    public interface IOssClientFactory
    {
        IOss Create(AliyunBlobProviderConfiguration args);
    }

}
