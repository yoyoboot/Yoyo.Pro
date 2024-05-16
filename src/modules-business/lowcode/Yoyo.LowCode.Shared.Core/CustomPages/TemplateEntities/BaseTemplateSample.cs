// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Yoyo.LowCode.CustomPages.TemplateEntities
{
    [Comment("样品模板表")]
    public class BaseTemplateSample : Entity<Guid>
    {
        [Comment("供方名称")]
        public string SupplierName { get; set; }

        [Comment("品号")]
        public string ProductNumber { get; set; }

        [Comment("材料名称")]
        public string MaterialName { get; set; }

        [Comment("型号规格")]
        public string ModelSpecifications { get; set; }

        [Comment("批号")]
        public string LotNumber { get; set; }

        [Comment("检验依据")]
        public string BasisForInspection { get; set; }

        [Comment("送样时间")]
        public DateTime? SampleDeliveryTime { get; set; }

        [Comment("送样数量")]
        public int? SampleNumber { get; set; }

        [Comment("基本信息")]
        public string BasicInformation { get; set; }

        [Comment("索样资质证明")]
        public string SampleQualificationCertificate { get; set; }

        [Comment("基础研发部会签意见")]
        public int? JCountersignComments { get; set; }

        [Comment("工程部会签意见")]
        public int? GCountersignComments { get; set; }

        [Comment("采购部会签意见")]
        public int? CCountersignComments { get; set; }

        [Comment("开发部会签意见")]
        public int? KCountersignComments { get; set; }

        [Comment("基础研发部签名")]
        public string JSignature { get; set; }

        [Comment("工程部签名")]
        public string GSignature { get; set; }

        [Comment("采购部签名")]
        public string CSignature { get; set; }

        [Comment("开发部签名")]
        public string KSignature { get; set; }

        [Comment("质量综述")]
        public string QualityOverview { get; set; }

        [MaxLength(LowCodeConsts.RemarkMaxCount)]
        [Comment("综合判断")]
        public int? ComprehensiveJudgment { get; set; }

        [Comment("品管部经理签名")]
        public string PSignature { get; set; }

        [Comment("审核日期")]
        public DateTime? AuditDate { get; set; }

        [Comment("编制日期")]
        public DateTime? WeaveDate { get; set; }
    }
}
