// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Yoyo.Pro.Dtos;

namespace Yoyo.LowCode.Databases.Dtos
{
    public class GetTableDataInput : PagedInputDto
    {
        /// <summary>
        ///     连接ID
        /// </summary>
        public Guid? DbLinkId { get; set; }

        public string Table { get; set; }
    }
}
