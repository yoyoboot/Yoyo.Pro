// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Yoyo.LowCode.Databases.Entity;
using Yoyo.LowCode.Entities.Auditing;
using Yoyo.Pro.EnumHelper;

namespace Yoyo.LowCode.LowCodeViewModels
{
    /// <summary>
    /// 关联关系配置表
    /// </summary>
    public class LowCodeModelRelation : LowCodeFullAuditedEntity<Guid>
    {
        public Guid? BaseCustomPageId { get; set; }

        /// <summary>
        /// 主表名称
        /// </summary>
        public string MainModelName { get; set; }

        /// <summary>
        /// 主表主键字段
        /// </summary>
        public string MainModelField { get; set; }

        /// <summary>
        /// 子表名称
        /// </summary>
        public string ChildModelName { get; set; }

        /// <summary>
        /// 子表外键字段
        /// </summary>
        public string ChildModelField { get; set; }

        public string ObjectRelationString { get; set; }

        private ObjectRelationEnum _objectRelation;

        /// <summary>
        /// 关联关系
        /// </summary>
        [Required]
        [Comment("关联关系")]
        public ObjectRelationEnum ObjectRelation
        {
            get
            {
                return _objectRelation;
            }
            set
            {
                _objectRelation = value;
                ObjectRelationString = _objectRelation.ToNameValue();
            }
        }
    }
}
