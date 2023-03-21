// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Yoyo.Pro.Entities.Auditing
{
    /// <summary>
    /// 删除用户名审计接口
    /// </summary>
    public interface IDeletionNameAudited
    {
        /// <summary>
        /// 删除用户名
        /// </summary>
        string DeleterUserName { get; set; }
    }
}
