using Abp.Dependency;
using Yoyo.Pro.AppFolders;

namespace Yoyo.Pro.AppProFolders
{
    public class AppProFolder : IAppProFolder, ISingletonDependency
    {
        readonly IBasicAppFolder _appFolder;

        public AppProFolder(IBasicAppFolder appFolder)
        {
            _appFolder = appFolder;
        }

        /// <summary>
        /// 文件管理的根目录
        /// </summary>
        public string SysFileRootFolder
        {
            get => this._appFolder.Get(nameof(SysFileRootFolder));
            set => this._appFolder.Set(nameof(SysFileRootFolder), value);
        }

        /// <summary>
        /// 文档管理（Wiki）的根目录
        /// </summary>
        public string ProjectRootFolder
        {
            get => this._appFolder.Get(nameof(ProjectRootFolder));
            set => this._appFolder.Set(nameof(ProjectRootFolder), value);
        }
    }
}
