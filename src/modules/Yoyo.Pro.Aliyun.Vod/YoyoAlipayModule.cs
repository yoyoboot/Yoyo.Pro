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
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

        }

        public override void PostInitialize()
        {

        }

       
    }
}
