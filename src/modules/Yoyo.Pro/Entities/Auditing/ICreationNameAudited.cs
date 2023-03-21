// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Yoyo.Pro.Entities.Auditing
{
    /// <summary>
    /// 创建用户名审计接口
    /// </summary>
    public interface ICreationNameAudited
    {
        /// <summary>
        /// 创建用户名
        /// </summary>
        string CreatorUserName { get; set; }
    }
}
