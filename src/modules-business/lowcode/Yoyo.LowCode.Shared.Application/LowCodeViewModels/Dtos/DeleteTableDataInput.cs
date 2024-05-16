// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Yoyo.LowCode.LowCodeViewModels.Dtos
{
    public class DeleteTableDataInput
    {
        /// <summary>
        ///     连接ID
        /// </summary>
        public Guid? DbLinkId { get; set; }

        /// <summary>
        ///     表名
        /// </summary>
        public string TableName { get; set; }
    }
}
