// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Yoyo.LowCode.Entities.Auditing;
using Yoyo.LowCode.UserQueryModule.UserQueryRelationalDefinition;

namespace Yoyo.LowCode.UserQueryModule.UserQueryGroupDefinition
{
    /// <summary>
    /// 查询组建模
    /// </summary>
    public class BaseUserQueryGroup : LowCodeFullAuditedEntity<Guid>
    {
        /// <summary>
        /// 查询组名称
        /// </summary>
        [StringLength(UserQueryGroupConsts.NameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        [StringLength(UserQueryGroupConsts.DescriptionLength)]
        public string Description { get; set; }

        /// <summary>
        /// 关联查询列表
        /// </summary>
        public ICollection<BaseUserQueryGroupEntity> Entitys { get; set; }

        /// <summary>
        /// 关联查询组
        /// </summary>
        public ICollection<BaseQueryGroupSub> SubGroups { get; set; }
    }
}
