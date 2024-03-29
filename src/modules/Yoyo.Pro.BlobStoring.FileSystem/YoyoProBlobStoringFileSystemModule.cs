using Abp.BlobStoring.FileSystem;
using Abp.Modules;


namespace Yoyo.Pro
{
    [DependsOn(typeof(AbpBlobStoringFileSystemModule))]
    public class YoyoProBlobStoringFileSystemModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(this.GetType().Assembly);
        }
    }
}
