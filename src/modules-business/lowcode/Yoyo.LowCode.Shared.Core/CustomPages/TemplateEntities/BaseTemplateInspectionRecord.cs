// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Abp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Yoyo.LowCode.CustomPages.TemplateEntities
{
    [Comment("检验记录模板表")]
    public class BaseTemplateInspectionRecord : Entity<Guid>
    {
        [Comment("检验项目")]
        public string InspectionItems { get; set; }

        [Comment("检验方法")]
        public string InspectionMethods { get; set; }

        [Comment("检验结果")]
        public string TestResults { get; set; }

        [Comment("责任部门")]
        public string ResponsibleDepartment { get; set; }

        [Comment("判断")]
        public string judgment { get; set; }

        [Comment("样品表Id")]
        public string SampleId { get; set; }
    }
}
