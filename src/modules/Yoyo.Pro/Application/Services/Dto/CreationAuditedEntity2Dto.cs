// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Abp.Application.Services.Dto;
using Yoyo.Pro.Entities.Auditing;

namespace Yoyo.Pro.Application.Services.Dto
{
    public class CreationAuditedEntity2Dto<TPrimaryKey> : CreationAuditedEntityDto<TPrimaryKey>, ICreationNameAudited
    {
        public virtual string CreatorUserName { get; set; }
    }
}
