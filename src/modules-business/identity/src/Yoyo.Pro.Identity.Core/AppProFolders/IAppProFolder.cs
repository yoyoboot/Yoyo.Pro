namespace Yoyo.Pro.AppProFolders
{
    public interface IAppProFolder
    {


        /// <summary>
        /// 文件管理的根目录
        /// </summary>
        string SysFileRootFolder { get; set; }

        /// <summary>
        /// 文档管理（Wiki）的根目录
        /// </summary>
        string ProjectRootFolder { get; set; }



    }
}
