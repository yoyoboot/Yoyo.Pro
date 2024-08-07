using Abp.BlobStoring;
using Abp.Modules;


namespace Yoyo.Pro
{
    [DependsOn(typeof(AbpBlobStoringModule))]
    public class YoyoProBlobStoringMinioModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(this.GetType().Assembly);
        }
    }
}
