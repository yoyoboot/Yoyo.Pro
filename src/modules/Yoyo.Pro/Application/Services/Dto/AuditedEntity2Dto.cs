// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Abp.Application.Services.Dto;
using Yoyo.Pro.Entities.Auditing;

namespace Yoyo.Pro.Application.Services.Dto
{
    public class AuditedEntity2Dto<TPrimaryKey> : AuditedEntityDto<TPrimaryKey>, IModificationNameAudited, ICreationNameAudited
    {
        public virtual string LastModifierUserName { get; set; }
        public virtual string CreatorUserName { get; set; }
    }
}
