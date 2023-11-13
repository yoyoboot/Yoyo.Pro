// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.EntityFrameworkCore;
using Yoyo.LowCode.CustomPages;
using Yoyo.LowCode.Entities.Auditing;

namespace Yoyo.LowCode.Databases.Entity
{
    /// <summary>
    ///     表关联
    /// </summary>
    [Comment("表关联")]
    public class BaseDbTableRelation : LowCodeFullAuditedEntity<Guid>
    {
        public Guid? BaseCustomPageId { get; set; }

        public BaseCustomPage BaseCustomPage { get; set; }

        /// <summary>
        ///     关联类型
        /// </summary>
        [Comment("关联类型")]
        public TableRelationEnum RelationType { get; set; }

        /// <summary>
        ///     关系类型
        /// </summary>
        [Comment("关系类型")]
        public ObjectRelationEnum ObjectRelationEnum { get; set; }

        /// <summary>
        ///     表名
        /// </summary>
        [Comment("表名")]
        public string Table { get; set; }

        /// <summary>
        ///     表注释
        /// </summary>
        [Comment("表注释")]
        public string TableName { get; set; }

        /// <summary>
        ///     外键字段
        /// </summary>
        [Comment("外键字段")]
        public string TableField { get; set; }

        /// <summary>
        ///     主键ID
        /// </summary>
        [Comment("主键字段")]
        public string TableKey { get; set; }

        /// <summary>
        ///     关联主表
        /// </summary>
        [Comment("关联主表")]
        public string RelationTable { get; set; }

        /// <summary>
        ///     关联主键
        /// </summary>
        [Comment("关联主键")]
        public string RelationField { get; set; }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            if (obj is not BaseDbTableRelation input)
            {
                return false;
            }

            return Id == input.Id;
        }
    }
}
