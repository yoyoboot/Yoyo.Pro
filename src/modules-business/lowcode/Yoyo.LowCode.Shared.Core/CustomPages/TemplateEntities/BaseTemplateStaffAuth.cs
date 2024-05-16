// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Abp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Yoyo.LowCode.CustomPages.TemplateEntities
{
    [Comment("人员认证")]
    public class BaseTemplateStaffAuth : Entity<Guid>
    {
        [Comment("操作员")]
        public string Operators { get; set; }

        [Comment("检验员")]
        public string Inspector { get; set; }

        [Comment("产品表Id")]
        public string TemplateProductsId { get; set; }
    }
}
