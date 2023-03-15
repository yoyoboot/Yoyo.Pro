using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yoyo.Pro.Configs
{


    /// <summary>
    /// 获取全局的配置中心
    /// </summary>
    public  partial class AbpAppConfig
    {

        

         
       
        /// <summary>
        /// gitlab图床配置
        /// </summary>
        public static List<GitlabConfig> GitlabConfigs { get; set; } = new List<GitlabConfig>();


        /// <summary>
        /// 获取文章中的分类配置文件
        /// </summary>
        public static List<PostCategoryConfig> PostCategoryConfigs { get; set; } = new List<PostCategoryConfig>();
        public static GitlabConfig GitlabConfig { get; set; } = new GitlabConfig();




    }
}
