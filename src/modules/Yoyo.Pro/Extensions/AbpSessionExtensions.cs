// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using Abp.Runtime.Session;
using Yoyo.Pro.Runtime.Session;

namespace Yoyo.Pro.Extensions
{
    public static class AbpSessionExtensions
    {
        /// <summary>
        /// 获取session中的用户名，需要AbpSesssion实现<see cref="IHasUserName"/>接口
        /// </summary>
        /// <param name="abpSession"></param>
        /// <returns></returns>
        public static string GetUserName(this IAbpSession abpSession)
        {
            if (abpSession is IHasUserName hasUserName)
            {
                return hasUserName.UserName;
            }

            return string.Empty;
        }
    }
}
