// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel.DataAnnotations;
using Yoyo.Pro.Entities;
using Yoyo.Pro.Entities.Auditing;

namespace Yoyo.LowCode.Entities.Auditing
{
    /// <summary>
    /// 具有增删改查业务的实体用
    /// </summary>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public abstract class LowCodeFullAuditedEntity<TPrimaryKey> : FullAuditedEntity2<TPrimaryKey>,
        IConcurrency
    {
        [MaxLength(32)]
        public string ConcurrencyToken { get; set; }
    }
}
