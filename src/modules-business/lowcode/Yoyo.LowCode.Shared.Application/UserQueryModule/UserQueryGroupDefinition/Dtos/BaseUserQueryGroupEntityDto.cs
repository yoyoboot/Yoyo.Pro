// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Abp.Application.Services.Dto;

namespace Yoyo.LowCode.UserQueryModule.UserQueryGroupDefinition.Dtos
{
    /// <summary>
    /// UserQueryGroup关联UserQuery中间表
    /// </summary>
    public class BaseUserQueryGroupEntityDto : EntityDto<Guid?>
    {
        /// <summary>
        /// 查询组Id
        /// </summary>
        public Guid UserQueryGroupId { get; set; }

        /// <summary>
        /// 查询建模Id
        /// </summary>
        public Guid UserQueryId { get; set; }
    }
}
