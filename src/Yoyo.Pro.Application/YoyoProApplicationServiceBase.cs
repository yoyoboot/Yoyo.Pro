using System.Reflection;
using Abp.Application.Services;
using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Modules;
using AutoMapper;
using Yoyo.Pro.Modules.DynamicView.Mapper;



namespace Yoyo.Pro
{
    public abstract class YoyoProApplicationServiceBase : ApplicationService
    {
        /// <summary>
        /// AutoMapper的配置提供者
        /// </summary>
        public IConfigurationProvider MapperProvider { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="localizationSourceName">本地化源名称</param>
        public YoyoProApplicationServiceBase(string localizationSourceName = null)
        {
            LocalizationSourceName = localizationSourceName ?? YoyoProConsts.LocalizationSourceName;
        }
    }
}
