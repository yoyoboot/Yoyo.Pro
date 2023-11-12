// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Yoyo.LowCode.UserQueryModule.UserQueryGroupDefinition.Dtos
{
    /// <summary>
    /// 创建或更新UserQuery Group
    /// </summary>
    public class BaseUserQueryGroupCreateOrUpdateInput
    {
        /// <summary>
        /// 实体Dto
        /// </summary>
        public BaseUserQueryGroupEditDto EntityDto { get; set; }
    }
}
