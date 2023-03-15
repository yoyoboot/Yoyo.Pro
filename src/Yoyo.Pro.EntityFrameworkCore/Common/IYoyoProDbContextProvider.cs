using System;
using System.Threading.Tasks;
using Abp.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace Yoyo.Pro.Common
{
    /// <summary>
    /// YoyoPro实现的 DbContext Provider
    /// </summary>
    public interface IYoyoProDbContextProvider
    {
        /// <summary>
        /// DbContext  类型
        /// </summary>
        Type DbContextType { get; }

        /// <summary>
        /// 获取 DbContext
        /// </summary>
        /// <returns></returns>
        DbContext GetDbContext();

        /// <summary>
        /// 带参数获取 DbContext
        /// </summary>
        /// <param name="multiTenancySide"></param>
        /// <returns></returns>
        DbContext GetDbContext(MultiTenancySides? multiTenancySide);

        /// <summary>
        /// 获取 DbContext 异步
        /// </summary>
        /// <returns></returns>
        Task<DbContext> GetDbContextAsync();

        /// <summary>
        /// 带参数获取 DbContext 异步
        /// </summary>
        /// <param name="multiTenancySide"></param>
        /// <returns></returns>
        Task<DbContext> GetDbContextAsync(MultiTenancySides? multiTenancySide);
    }
}
