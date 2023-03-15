using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Yoyo.Pro
{

    /// <summary>
    /// 获取配置信息的帮助类
    /// </summary>
    public static  class ConfigHelper
    {
        private static readonly IConfiguration AppConfiguration;
 
        static ConfigHelper()
        {              
            
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            AppConfiguration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true)
                .AddJsonFile($"appsettings.{environment}.json", true)
                .Build();
        }

        public static T GetSection<T>(string key) where T : class, new()
        {
            var obj = new ServiceCollection()
                .AddOptions()
                .Configure<T>(AppConfiguration.GetSection(key))
                .BuildServiceProvider()
                .GetService<IOptions<T>>()
                .Value;
            return obj;
        }

        public static string GetSection(string key)
        {
            return AppConfiguration.GetValue<string>(key);
        }


    }
}
