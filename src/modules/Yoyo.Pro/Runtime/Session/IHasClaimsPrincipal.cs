// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Security.Claims;

namespace Yoyo.Pro.Runtime.Session
{
    /// <summary>
    /// AbpSession 扩展，访问 ClaimsPrincipal
    /// </summary>
    public interface IHasClaimsPrincipal
    {
        /// <summary>
        /// ClaimsPrincipal实例
        /// </summary>
        ClaimsPrincipal ClaimsPrincipal { get; }

        /// <summary>
        /// 根据claim type获取ClaimsPrincipal中的值
        /// </summary>
        /// <param name="claimType"></param>
        /// <returns></returns>
        string GetClaimValue(string claimType);
    }
}
