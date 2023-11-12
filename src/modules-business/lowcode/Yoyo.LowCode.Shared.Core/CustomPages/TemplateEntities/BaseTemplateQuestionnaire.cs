// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Yoyo.LowCode.CustomPages.TemplateEntities
{
    [Comment("问卷调查模板表")]
    public class BaseTemplateQuestionnaire : Entity<Guid>
    {
        [Comment("性别")]
        public int? Gender { get; set; }

        [Comment("年龄段")]
        public int? AgeGroups { get; set; }

        [Comment("人生观的了解程度")]
        public int? Philosophy { get; set; }

        [Comment("人生观对于社会发展的影响")]
        public int? SocialImpact { get; set; }

        [Comment("生活满意程度")]
        public int? LifeSatisfaction { get; set; }

        [Comment("人生的规划")]
        public int? Lifeplanning { get; set; }

        [Comment("对于人生影响的主要因素")]
        public int? MajorImpact { get; set; }

        [Comment("向往的生活")]
        public int? Yearning { get; set; }

        [MaxLength(LowCodeConsts.RemarkMaxCount)]
        [Comment("模板注释")]
        public string suggestion { get; set; }
    }
}
