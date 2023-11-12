// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Yoyo.LowCode.LowCodeAPI.Dtos.ApplyTypeDto
{
    public class ApplyTypeEditDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// 应用分类名称(内置http:1001  Sqlserver:1002)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 应用分类编码(内置http:1001  Sqlserver:1002)
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 乐观锁Token
        /// </summary>
        public string ConcurrencyToken { get; set; }
    }
}
