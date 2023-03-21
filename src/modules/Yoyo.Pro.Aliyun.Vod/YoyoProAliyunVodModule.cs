using System.Reflection;
using Abp;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Yoyo.Pro
{
    /// <summary>
    /// YoyoSoft Abp 阿里云VOD模块
    /// </summary>
    
    public class YoyoProAliyunVodModule : AbpModule
    {
        public override void PreInitialize()
        {
         
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(YoyoProAliyunVodModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

        }

        public override void PostInitialize()
        {

        }

       
    }
}
