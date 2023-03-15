using System.Text.RegularExpressions;
using Abp.Auditing;
using Abp.Configuration.Startup;
using Yoyo.Pro.Auditing;

namespace Yoyo.Pro.Configuration
{

    /// <summary>
    /// YoyoPro 对 Configuration 的扩展，用于配置 Auditing
    /// </summary>
    public static class YoyoProAuditingConfigurationMongoExtensions
    {
        public const string DATABASE_NAME_REGEX = @".*\/(.*?)\?authSource";
        public const string DEFAULT_DATABASE_NAME = @"YoyoProAuditLog";

        public const string YoyoPro_MONGO_CONNECTION_STRING = nameof(YoyoPro_MONGO_CONNECTION_STRING);
        public const string YoyoPro_MONGO_DATABASE_NAME = nameof(YoyoPro_MONGO_DATABASE_NAME);


        /// <summary>
        /// 使用YoyoPro实现的审计日志存储-mongodb
        /// </summary>
        /// <param name="YoyoProAuditingConfigurations">配置信息</param>
        /// <param name="mongoConnectionString">连接字符串</param>
        /// <returns></returns>
        public static IYoyoProAuditingConfiguration UseMongoProvider(this IYoyoProAuditingConfiguration YoyoProAuditingConfigurations, string mongoConnectionString)
        {
            return YoyoProAuditingConfigurations.UseMongoProvider<AuditLogs2>(mongoConnectionString);
        }

        /// <summary>
        /// 使用YoyoPro实现的审计日志存储-mongodb
        /// </summary>
        /// <typeparam name="TAuditLog">实体类型</typeparam>
        /// <param name="YoyoProAuditingConfigurations">配置信息</param>
        /// <param name="mongoConnectionString">连接字符串</param>
        /// <returns></returns>
        public static IYoyoProAuditingConfiguration UseMongoProvider<TAuditLog>(this IYoyoProAuditingConfiguration YoyoProAuditingConfigurations, string mongoConnectionString)
                 where TAuditLog : AuditLogs2, new()
        {
            YoyoProAuditingConfigurations.UseAuditLogRepositoryType<MongoAuditLogRepository<TAuditLog>, TAuditLog>();
            YoyoProAuditingConfigurations.SetMongoConnectionString(mongoConnectionString);
            return YoyoProAuditingConfigurations;
        }

        /// <summary>
        /// 设置mongo的连接字符串
        /// </summary>
        /// <param name="YoyoProAuditingConfigurations"></param>
        /// <param name="mongoConnectionString"></param>
        /// <returns></returns>
        public static IYoyoProAuditingConfiguration SetMongoConnectionString(this IYoyoProAuditingConfiguration YoyoProAuditingConfigurations, string mongoConnectionString)
        {
            YoyoProAuditingConfigurations.CustomData[YoyoPro_MONGO_CONNECTION_STRING] = mongoConnectionString;

            // 解析连接字符串中的数据库名称
            YoyoProAuditingConfigurations.SetMongoDatabaseName(mongoConnectionString.GetMongoDatabaseNameFromConnectionString());

            return YoyoProAuditingConfigurations;
        }

        /// <summary>
        /// 获取mongo的连接字符串
        /// </summary>
        /// <param name="YoyoProAuditingConfigurations"></param>
        /// <returns></returns>
        public static string GetMongoConnectionString(this IYoyoProAuditingConfiguration YoyoProAuditingConfigurations)
        {
            return YoyoProAuditingConfigurations.CustomData[YoyoPro_MONGO_CONNECTION_STRING].ToString();
        }

        /// <summary>
        /// 设置使用的数据库名称
        /// </summary>
        /// <param name="YoyoProAuditingConfigurations"></param>
        /// <param name="databaseName"></param>
        /// <returns></returns>
        public static IYoyoProAuditingConfiguration SetMongoDatabaseName(this IYoyoProAuditingConfiguration YoyoProAuditingConfigurations, string databaseName)
        {
            YoyoProAuditingConfigurations.CustomData[YoyoPro_MONGO_DATABASE_NAME] = databaseName;
            return YoyoProAuditingConfigurations;
        }

        /// <summary>
        /// 获取使用的数据库名称
        /// </summary>
        /// <param name="YoyoProAuditingConfigurations"></param>
        /// <returns></returns>
        public static string GetMongoDatabaseName(this IYoyoProAuditingConfiguration YoyoProAuditingConfigurations)
        {
            if (!YoyoProAuditingConfigurations.CustomData.TryGetValue(YoyoPro_MONGO_DATABASE_NAME, out var databaseName))
            {
                YoyoProAuditingConfigurations.SetMongoDatabaseName(DEFAULT_DATABASE_NAME);
                databaseName = DEFAULT_DATABASE_NAME;
            }

            return databaseName.ToString();
        }

        /// <summary>
        /// 从连接字符串中匹配连接的数据库名称
        /// </summary>
        /// <param name="mongoConnectionString"></param>
        /// <returns></returns>
        public static string GetMongoDatabaseNameFromConnectionString(this string mongoConnectionString)
        {
            var match = Regex.Match(mongoConnectionString, DATABASE_NAME_REGEX);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            return DEFAULT_DATABASE_NAME;
        }
    }

}
