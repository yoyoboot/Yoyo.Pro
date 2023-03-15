// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using Abp.Extensions;

namespace Yoyo.Pro.Extensions
{
    public static class StringUrlExtensions
    {
        /// <summary>
        /// 是否为外部链接
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsExternalLink(this string url)
        {
            if (url.IsNullOrEmpty())
            {
                return false;
            }

            return url.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
                || url.StartsWith("https://", StringComparison.OrdinalIgnoreCase);
        }
    }
}
