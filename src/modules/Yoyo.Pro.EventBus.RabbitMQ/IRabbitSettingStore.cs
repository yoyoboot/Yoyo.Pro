using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Yoyo.Pro
{

    /// <summary>
    /// RabbitMQ 配置存储器
    /// </summary>
    public interface IRabbitSettingStore : IDisposable
    {
        /// <summary>
        /// 根据配置名称获取连接工厂
        /// </summary>
        /// <param name="configName">配置名称</param>
        /// <returns></returns>
        ConnectionFactory GetConnectionFactory(string configName = null);


        /// <summary>
        /// 配置连接工厂
        /// </summary>
        /// <param name="configName">配置名称</param>
        /// <param name="connectionFactory">工厂实例</param>
        void SetConnectionFactory(string configName, ConnectionFactory connectionFactory);

    }


    public class RabbitSettingStore : IRabbitSettingStore
    {

        protected readonly ILogger<IRabbitSettingStore> _logger;


        protected readonly ConcurrentDictionary<string, ConnectionFactory> _dataDict;

        public RabbitSettingStore(ILogger<IRabbitSettingStore> logger = null)
        {
            _logger = logger;
            _dataDict = new ConcurrentDictionary<string, ConnectionFactory>();
        }

        public virtual ConnectionFactory GetConnectionFactory(string configName = null)
        {
            if (string.IsNullOrWhiteSpace(configName))
            {
                configName = RabbitConsts.DefaultConfigName;
            }

            _dataDict.TryGetValue(configName, out var connectionFactory);
            return connectionFactory;
        }

        public virtual void SetConnectionFactory(string configName, ConnectionFactory connectionFactory)
        {
            if (string.IsNullOrWhiteSpace(configName))
            {
                configName = RabbitConsts.DefaultConfigName;
            }

            _dataDict.AddOrUpdate(configName, connectionFactory, (key, OldVal) =>
            {
                return connectionFactory;
            });
        }



        public void Dispose()
        {
            _dataDict.Clear();
        }
    }


}
