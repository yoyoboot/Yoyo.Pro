// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Abp.Application.Services.Dto;

namespace Yoyo.LowCode.UserQueryModule.UserQueryGroupDefinition.Dtos
{
    /// <summary>
    /// UserQueryGroup关联UserQueryGroup中间表
    /// </summary>
    public class BaseUserQueryGroupSubDto : EntityDto<Guid?>
    {
        /// <summary>
        /// 查询组Id
        /// </summary>
        public Guid UserQueryGroupId { get; set; }

        /// <summary>
        /// 子查询组Id
        /// </summary>
        public Guid SubUserQueryGroupId { get; set; }
    }
}
