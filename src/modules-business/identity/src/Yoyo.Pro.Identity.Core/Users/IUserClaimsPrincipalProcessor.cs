// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Security.Claims;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.Dependency;

namespace Yoyo.Pro.Users
{
    /// <summary>
    /// UserClaimsPrincipal 处理器 
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public interface IUserClaimsPrincipalProcessor<TUser>
        where TUser : AbpUser<TUser>
    {
        /// <summary>
        /// 创建方法，处理claimsPrincipal的内容
        /// </summary>
        /// <param name="claimsPrincipal">要附加的内容</param>
        /// <param name="user">信息</param>
        /// <returns></returns>
        Task Run(ClaimsPrincipal claimsPrincipal, TUser user);
    }

    /// <summary>
    /// UserClaimsPrincipal 处理器基类
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public abstract class UserClaimsPrincipalProcessorBase<TUser>
            : IUserClaimsPrincipalProcessor<TUser>, ISingletonDependency
        where TUser : AbpUser<TUser>
    {
        public abstract Task Run(ClaimsPrincipal claimsPrincipal, TUser user);
    }
}
