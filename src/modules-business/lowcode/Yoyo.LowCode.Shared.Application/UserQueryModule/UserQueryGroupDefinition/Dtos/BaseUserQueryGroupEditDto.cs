// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Yoyo.LowCode.Dtos;

namespace Yoyo.LowCode.UserQueryModule.UserQueryGroupDefinition.Dtos
{
    /// <summary>
    /// BaseUserQueryGroupEditDto
    /// </summary>
    public class BaseUserQueryGroupEditDto : LowCodeFullAuditedEntityDto<Guid?>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 关联查询列表
        /// </summary>
        public ICollection<BaseUserQueryGroupEntityDto> Entitys { get; set; }

        /// <summary>
        /// 关联查询组
        /// </summary>
        public ICollection<BaseUserQueryGroupSubDto> SubGroups { get; set; }
    }
}
