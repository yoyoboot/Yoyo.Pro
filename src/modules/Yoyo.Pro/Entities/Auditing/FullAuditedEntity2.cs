// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities.Auditing;

namespace Yoyo.Pro.Entities.Auditing
{
    /// <summary>
    /// 创建、修改、删除 用户名审计基类
    /// </summary>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public abstract class FullAuditedEntity2<TPrimaryKey> : FullAuditedEntity<TPrimaryKey>,
        IDeletionNameAudited,
        ICreationNameAudited,
        IModificationNameAudited
    {
        /// <inheritdoc/>
        [MaxLength(32)]
        public virtual string DeleterUserName { get; set; }

        /// <inheritdoc/>
        [MaxLength(32)]
        public virtual string CreatorUserName { get; set; }

        /// <inheritdoc/>
        [MaxLength(32)]
        public virtual string LastModifierUserName { get; set; }
    }
}
