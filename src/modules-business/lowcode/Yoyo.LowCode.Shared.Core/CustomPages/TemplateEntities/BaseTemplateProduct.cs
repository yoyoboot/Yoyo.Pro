// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;

namespace Yoyo.LowCode.CustomPages.TemplateEntities
{
    public class BaseTemplateProduct : Entity<Guid>
    {
        public string ProductNumber { get; set; }

        public string ProductionLotNumber { get; set; }

        public int? Quantity { get; set; }

        public string DeviceNumber { get; set; }

        public int? temperature { get; set; }

        public DateTime Time { get; set; }

        public int? Pressure { get; set; }

        [MaxLength(LowCodeConsts.RemarkMaxCount)]
        public string Remark { get; set; }
    }
}
