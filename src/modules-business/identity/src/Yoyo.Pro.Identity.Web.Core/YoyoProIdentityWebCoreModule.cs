using System.IO;
using Abp.AspNetCore.Configuration;
using Abp.IO;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Yoyo.Pro;
using Yoyo.Pro.AppProFolders;
using Microsoft.AspNetCore.Hosting;

namespace Yoyo.Pro
{
    [DependsOn(typeof(YoyoProWebCoreModule),
        typeof(YoyoProIdentityApplicationModule),
        typeof(YoyoProIdentityEntityFrameworkCoreModule))]
    public class YoyoProIdentityWebCoreModule : AbpModule
    {
        private readonly IWebHostEnvironment _env;

        public YoyoProIdentityWebCoreModule(IWebHostEnvironment env)
        {
            _env = env;

        }

        public override void PreInitialize()
        {
            Configuration.Modules.AbpAspNetCore()
                .CreateControllersForAppServices(typeof(YoyoProIdentityApplicationModule).GetAssembly(), "identity");
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(GetType().GetAssembly());
        }

        public override void PostInitialize()
        {
            this.SetAppFolders();
        }

        public override void Shutdown()
        {
        }

        /// <summary>
        /// 启动项目的时候设置存档的文件夹信息
        /// </summary>
        protected virtual void SetAppFolders()
        {
            var appFolders = IocManager.Resolve<IAppProFolder>();
            appFolders.SysFileRootFolder = Path.Combine(_env.WebRootPath, $"SysFiles");
            appFolders.ProjectRootFolder = Path.Combine(_env.WebRootPath, $"Projects");
            try
            {
                DirectoryHelper.CreateIfNotExists(appFolders.ProjectRootFolder);
                DirectoryHelper.CreateIfNotExists(appFolders.SysFileRootFolder);
            }
            catch
            {
                // ignored
            }
        }
    }
}
