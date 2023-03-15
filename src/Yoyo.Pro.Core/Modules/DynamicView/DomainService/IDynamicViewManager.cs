using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Abp.Domain.Services;

namespace Yoyo.Pro.Modules.DynamicView.DomainService
{
    /// <summary>
    /// 动态页面配置信息获取
    /// </summary>
    public interface IDynamicViewManager : IDomainService
    {
        /// <summary>
        /// 获取动态页面配置信息
        /// </summary>
        /// <param name="name">动态配置名称</param>
        /// <returns>页面配置</returns>
        Task<DynamicPage> GetDynamicPageInfo(string name);

        /// <summary>
        /// 根据名称获取对应的筛选条件配置
        /// </summary>
        /// <param name="name">筛选条件名称</param>
        /// <returns>筛选条件配置</returns>
        Task<IEnumerable<PageFilterItem>> GetPageFilters(string name);



        /// <summary>
        /// 根据名称获取对应的筛选条件配置
        /// </summary>
        /// <param name="name">筛选条件名称</param>
        /// <returns>筛选条件配置</returns>
        Task<IEnumerable<ColumnItem>> GetColumns(string name);


        /// <summary>
        /// 扫描程序集中的enum
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <returns></returns>
        IDynamicViewManager AddEnum(Assembly assembly);

        /// <summary>
        /// 添加枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IDynamicViewManager AddEnum<T>();

        /// <summary>
        /// 获取枚举类型
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Type GetEnumType(string name);
    }
}
