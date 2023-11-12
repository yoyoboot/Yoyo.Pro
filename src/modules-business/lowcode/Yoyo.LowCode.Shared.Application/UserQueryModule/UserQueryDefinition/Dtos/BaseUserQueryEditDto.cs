// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Yoyo.LowCode.Dtos;

namespace Yoyo.LowCode.UserQueryModule.UserQueryDefinition.Dtos
{
    /// <summary>
    /// UserQueryDto
    /// </summary>
    public class BaseUserQueryEditDto : LowCodeFullAuditedEntityDto<Guid?>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 查询模板
        /// </summary>
        public string QueryTemplate { get; set; }

        /// <summary>
        /// 查询参数
        /// </summary>
        public List<BaseUserQueryParamter> QueryParamters { get; set; }
    }
}
