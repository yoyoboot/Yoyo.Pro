// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Yoyo.Pro
{
    public interface IDatabaseChecker
    {
        /// <summary>
        /// 判断数据库是否存在
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <returns></returns>
        bool Exist(string connectionString);

        /// <summary>
        /// 获取当前的数据库上下文实例
        /// </summary>
        /// <returns></returns>
        DbContext GetDbContext();
    }
}
