// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Yoyo.LowCode.Dtos
{
    public class LowCodeNdoDto
    {

        /// <summary>
        /// 编号
        /// </summary>
        public virtual Guid Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreationTime { get; set; }
    }
}
