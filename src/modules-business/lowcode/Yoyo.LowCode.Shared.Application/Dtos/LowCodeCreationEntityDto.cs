// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using Yoyo.Pro.Entities.Auditing;

namespace Yoyo.LowCode.Dtos
{
    /// <summary>
    /// 创建实体Dto
    /// </summary>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public abstract class LowCodeCreationEntityDto<TPrimaryKey> : EntityDto<TPrimaryKey>,
        IHasCreationTime,
        ICreationNameAudited
    {
        /// <summary>
        /// 创建人
        /// </summary>
        public virtual string CreatorUserName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreationTime { get; set; }
    }
}
