// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text;

namespace Yoyo.LowCode.Renders.Dtos
{
    public class ProcessSqlDto
    {
        /// <summary>
        /// 主表sql
        /// </summary>
        public StringBuilder mainSql { get; set; }

        /// <summary>
        /// 子表sql
        /// </summary>
        public StringBuilder childSql { get; set; }

        /// <summary>
        /// 要删除的Id
        /// </summary>
        public string deleteId { get; set; }
    }
}
