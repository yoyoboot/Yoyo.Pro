// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Yoyo.LowCode.Databases.Entity;

namespace Yoyo.LowCode.CustomPages.Dtos
{
    public class DbTableRelationEditDto
    {
        public Guid? Id { get; set; }

        /// <summary>
        /// 关联类型
        /// </summary>
        public TableRelationEnum RelationType { get; set; }

        /// <summary>
        /// 关系类型
        /// </summary>
        public ObjectRelationEnum ObjectRelationEnum { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        public string Table { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        public string TableKey { get; set; }

        /// <summary>
        /// 外键字段
        /// </summary>
        public string TableField { get; set; }

        /// <summary>
        /// 关联主表
        /// </summary>
        public string RelationTable { get; set; }

        /// <summary>
        /// 关联主键
        /// </summary>
        public string RelationField { get; set; }
    }
}
