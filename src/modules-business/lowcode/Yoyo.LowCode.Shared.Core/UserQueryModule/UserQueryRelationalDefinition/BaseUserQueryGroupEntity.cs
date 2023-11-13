// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Abp.Domain.Entities;
using Yoyo.LowCode.UserQueryModule.UserQueryDefinition;
using Yoyo.LowCode.UserQueryModule.UserQueryGroupDefinition;

namespace Yoyo.LowCode.UserQueryModule.UserQueryRelationalDefinition
{
    /// <summary>
    /// 查询组关联查询
    /// </summary>
    public class BaseUserQueryGroupEntity : Entity<Guid>, ISoftDelete
    {
        #region Public Properties

        /// <summary>
        /// 逻辑删除标记
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 查询建模
        /// </summary>
        public BaseUserQuery BaseUserQuery { get; set; }

        /// <summary>
        /// 查询组
        /// </summary>
        public BaseUserQueryGroup BaseUserQueryGroup { get; set; }

        /// <summary>
        /// 查询组Id
        /// </summary>
        public Guid UserQueryGroupId { get; set; }

        /// <summary>
        /// 查询建模Id
        /// </summary>
        public Guid UserQueryId { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override bool Equals(object? obj)
        {
            if (obj is not BaseUserQueryGroupEntity input)
            {
                return false;
            }

            return this.Id == input.Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        #endregion Public Methods
    }
}
