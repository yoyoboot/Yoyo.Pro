// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities.Auditing;

namespace Yoyo.Pro.Entities.Auditing
{
    /// <summary>
    /// 创建和修改用户名审计实体基类
    /// </summary>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public abstract class AuditedEntity2<TPrimaryKey> : AuditedEntity<TPrimaryKey>, IModificationNameAudited, ICreationNameAudited
    {
        /// <inheritdoc/>
        [MaxLength(32)]
        public virtual string LastModifierUserName { get; set; }

        /// <inheritdoc/>
        [MaxLength(32)]
        public virtual string CreatorUserName { get; set; }
    }
}
