namespace Yoyo.Pro.BlobStoring
{
    public interface IMinioBlobNameCalculator
    {
        string Calculate(BlobProviderArgs args);
    }

}
