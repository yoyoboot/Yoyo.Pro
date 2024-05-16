// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Yoyo.LowCode.Entities.Auditing;

namespace Yoyo.LowCode.UserQueryModule.UserQueryDefinition
{
    /// <summary>
    /// 查询建模
    /// </summary>
    public class BaseUserQuery : LowCodeFullAuditedEntity<Guid>
    {
        /// <summary>
        /// 查询名称
        /// </summary>
        [StringLength(UserQueryConsts.NameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        [StringLength(UserQueryConsts.DescriptionLength)]
        public string Description { get; set; }

        /// <summary>
        /// 查询模板
        /// </summary>
        public string QueryTemplate { get; set; }

        /// <summary>
        /// 查询参数
        /// </summary>
        public List<BaseUserQueryParamter> QueryParamters { get; set; }

        /// <summary>
        /// 获取查询参数
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetQueryParamters()
        {
            return this.QueryParamters.ToDictionary(o => o.Name, o => o.GetValForType());
        }
    }
}
