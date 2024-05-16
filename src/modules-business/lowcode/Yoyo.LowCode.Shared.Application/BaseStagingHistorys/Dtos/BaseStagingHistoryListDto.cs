// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Abp.Domain.Entities.Auditing;

namespace Yoyo.LowCode.BaseStagingHistorys.Dtos
{
    public class BaseStagingHistoryListDto : CreationAuditedEntity<Guid>
    {
        public Guid Key { get; set; }

        public string StagingJson { get; set; }

        public long? UserId { get; set; }
    }
}
