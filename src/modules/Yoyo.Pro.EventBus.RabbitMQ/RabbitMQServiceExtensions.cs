using Microsoft.Extensions.DependencyInjection;

namespace Yoyo.Pro
{
    public static class RabbitMQServiceExtensions
    {
        /// <summary>
        /// 添加RabbitMq服务注册
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddRabbitMQService(this IServiceCollection services)
        {
            services.AddSingleton<IRabbitConnectionStore, RabbitConnectionStore>();
            services.AddSingleton<IRabbitPolicyStore, RabbitPolicyStore>();
            services.AddSingleton<IRabbitSettingStore, RabbitSettingStore>();
            // 注册序列化服务
            services.AddSingleton<IMsgPackSerializer, MsgPackSerializer>();
            // 注册发布订阅服务
            services.AddTransient<IRabbitSubscriber, RabbitSubscriber>();
            services.AddTransient<IRabbitPublisher, RabbitPublisher>();

            return services;
        }
    }
}
