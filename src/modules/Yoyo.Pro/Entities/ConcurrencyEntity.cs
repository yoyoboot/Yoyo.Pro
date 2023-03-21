// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;

namespace Yoyo.Pro.Entities
{
    /// <summary>
    /// 并发实体基类
    /// </summary>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public abstract class ConcurrencyEntity<TPrimaryKey> : Entity<TPrimaryKey>, IConcurrency
    {
        ///<inheritdoc/>
        [MaxLength(32)]
        public virtual string ConcurrencyToken { get; set; }
    }
}
