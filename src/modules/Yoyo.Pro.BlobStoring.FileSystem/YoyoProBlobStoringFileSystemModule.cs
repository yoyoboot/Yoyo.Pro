using Abp.Modules;


namespace Yoyo.Pro
{
    [DependsOn(typeof(YoyoProBlobStoringModule))]
    public class YoyoProBlobStoringFileSystemModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(this.GetType().Assembly);
        }
    }
}
