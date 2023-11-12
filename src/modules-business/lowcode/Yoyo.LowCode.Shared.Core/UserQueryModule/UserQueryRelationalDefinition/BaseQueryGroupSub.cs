// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Yoyo.LowCode.UserQueryModule.UserQueryGroupDefinition;

namespace Yoyo.LowCode.UserQueryModule.UserQueryRelationalDefinition
{
    /// <summary>
    /// 查询组关联查询组
    /// </summary>
    public class BaseQueryGroupSub : Entity<Guid>, ISoftDelete
    {
        /// <summary>
        /// 查询组Id
        /// </summary>
        public Guid UserQueryGroupId { get; set; }

        /// <summary>
        /// 查询组
        /// </summary>
        public BaseUserQueryGroup BaseUserQueryGroup { get; set; }

        /// <summary>
        /// 子查询组Id
        /// </summary>
        public Guid SubUserQueryGroupId { get; set; }

        /// <summary>
        /// 子查询组
        /// </summary>
        [NotMapped]
        public BaseUserQueryGroup SubBaseUserQueryGroup { get; set; }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            if (obj is not BaseQueryGroupSub input)
            {
                return false;
            }

            return this.Id == input.Id;
        }

        /// <summary>
        /// 逻辑删除标记
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
