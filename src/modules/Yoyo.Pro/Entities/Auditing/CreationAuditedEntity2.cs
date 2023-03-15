// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities.Auditing;

namespace Yoyo.Pro.Entities.Auditing
{
    /// <summary>
    /// 创建用户名审计的基类
    /// </summary>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public abstract class CreationAuditedEntity2<TPrimaryKey> : CreationAuditedEntity<TPrimaryKey>, ICreationNameAudited
    {
        /// <inheritdoc/>
        [MaxLength(32)]
        public virtual string CreatorUserName { get; set; }
    }
}
