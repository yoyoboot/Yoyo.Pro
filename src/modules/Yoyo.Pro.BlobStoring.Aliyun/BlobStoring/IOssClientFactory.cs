
using Aliyun.OSS;

using System.Collections.Generic;
using System.Text;

namespace Yoyo.Pro.BlobStoring
{
    public interface IOssClientFactory
    {
        IOss Create(AliyunBlobProviderConfiguration args);
    }

}
