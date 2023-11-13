// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Abp.Application.Services.Dto;
using Yoyo.Pro.Entities;
using Yoyo.Pro.Entities.Auditing;

namespace Yoyo.LowCode.Dtos
{
    /// <summary>
    /// 全审计实体Dto
    /// </summary>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public abstract class LowCodeFullAuditedEntityDto<TPrimaryKey> : FullAuditedEntityDto<TPrimaryKey>,
        IDeletionNameAudited,
        ICreationNameAudited,
        IModificationNameAudited,
        IConcurrency
    {
        /// <summary>
        /// 删除人
        /// </summary>
        public virtual string DeleterUserName { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public virtual string CreatorUserName { get; set; }

        /// <summary>
        /// 最后更新人
        /// </summary>
        public virtual string LastModifierUserName { get; set; }

        /// <summary>
        /// 并发字段
        /// </summary>
        public virtual string ConcurrencyToken { get; set; }
    }
}
