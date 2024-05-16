// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Yoyo.LowCode.Databases.Entity;

namespace Yoyo.LowCode.LowCodeViewModels.Dtos
{
    public class LowCodeModelRelationEditDto
    {
        public Guid? Id { get; set; }

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

        /// <summary>
        /// 关联关系
        /// </summary>
        public ObjectRelationEnum ObjectRelation { get; set; }
    }
}
