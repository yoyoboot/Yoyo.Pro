using Abp.Modules;


namespace Yoyo.Pro
{
    [DependsOn(typeof(YoyoProBlobStoringModule))]
    public class YoyoProBlobStoringMinioModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(this.GetType().Assembly);
        }
    }
}
