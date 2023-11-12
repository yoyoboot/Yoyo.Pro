// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Yoyo.LowCode.UserQueryModule.UserQueryDefinition.Dtos
{
    /// <summary>
    /// BaseUserQuery 测试输出结果
    /// </summary>
    public class BaseUserQueryTestOutput
    {
        /// <summary>
        /// 列定义
        /// </summary>
        public List<string> Columns { get; set; }

        /// <summary>
        /// 查询输出数据
        /// </summary>
        public List<List<object>> Rows { get; set; }
    }
}
