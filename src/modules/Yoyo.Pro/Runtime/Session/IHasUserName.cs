// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using Abp.Runtime.Session;

namespace Yoyo.Pro.Runtime.Session
{
    /// <summary>
    /// AbpSession 扩展，包含用户名
    /// </summary>
    public interface IHasUserName
    {
        /// <summary>
        /// 当前登录用户名
        /// </summary>
        public string UserName { get; }
    }
}
