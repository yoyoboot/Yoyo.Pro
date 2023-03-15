namespace Yoyo.Pro.BlobStoring
{
    public interface IBlobFilePathCalculator
    {
        string Calculate(BlobProviderArgs args);
    }

    
}
