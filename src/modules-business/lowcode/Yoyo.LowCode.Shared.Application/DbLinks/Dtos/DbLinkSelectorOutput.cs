// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using SqlSugar;

namespace Yoyo.LowCode.DbLinks.Dtos
{
    public class DbLinkSelectorOutput
    {
        public Guid Id { get; set; }

        /// <summary>
        ///     数据库类型
        /// </summary>
        public DbType DbType { get; set; }

        /// <summary>
        ///     库名
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        ///     是否是默认数据库
        /// </summary>
        public bool IsDefault { get; set; } = false;
    }
}
