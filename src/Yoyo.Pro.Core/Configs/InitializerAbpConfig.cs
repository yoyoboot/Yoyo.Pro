using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Yoyo.Pro.Configs
{



    /// <summary>
    /// 初始化服务的方法
    /// </summary>
    public static class InitializerAbpConfig
    {
        public static IApplicationBuilder UseBindAbpConfig(this IApplicationBuilder builder)
        {
            var scope = builder.ApplicationServices.CreateScope();

            var configuration = scope.ServiceProvider.GetService<IConfiguration>();

            // Imgbed
            configuration.Bind("Imgbed:Gitlab", AbpAppConfig.GitlabConfigs);
            configuration.Bind("MarkdownPosts:Categories", AbpAppConfig.PostCategoryConfigs);

            return builder;
        }
    }
}
