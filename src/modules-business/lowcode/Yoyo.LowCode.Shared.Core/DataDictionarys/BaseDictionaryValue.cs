// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Yoyo.LowCode.Entities.Auditing;

namespace Yoyo.LowCode.DataDictionarys
{
    /// <summary>
    /// 数据字典值
    /// </summary>
    public class BaseDictionaryValue : LowCodeFullAuditedEntity<Guid>
    {
        public Guid BaseDictionaryTypeId { get; set; }

        public BaseDictionaryType BaseDictionaryType { get; set; }

        /// <summary>
        /// 代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 顺序号
        /// </summary>
        public int? OrderNo { get; set; }

        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool IsDisabled { get; set; }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            if (obj is not BaseDictionaryValue input)
            {
                return false;
            }

            return this.Id == input.Id;
        }
    }
}
