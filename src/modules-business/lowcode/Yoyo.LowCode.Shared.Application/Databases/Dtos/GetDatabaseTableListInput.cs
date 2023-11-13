// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Yoyo.LowCode.Databases.Dtos
{
    public class GetDatabaseTableListInput
    {
        /// <summary>筛选文本</summary>
        public virtual string FilterText { get; set; }

        /// <summary>
        /// 连接ID
        /// </summary>
        public Guid? DbLinkId { get; set; }

        public bool IsFilterDefault { get; set; }
    }
}
