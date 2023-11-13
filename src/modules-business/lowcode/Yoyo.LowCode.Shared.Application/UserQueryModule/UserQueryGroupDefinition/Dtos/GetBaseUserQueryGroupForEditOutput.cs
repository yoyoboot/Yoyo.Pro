// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Yoyo.LowCode.UserQueryModule.UserQueryGroupDefinition.Dtos
{
    /// <summary>
    /// 获取编辑信息
    /// </summary>
    public class GetBaseUserQueryGroupForEditOutput
    {
        /// <summary>
        /// 实体Dto
        /// </summary>
        public BaseUserQueryGroupEditDto EntityDto { get; set; }

        public string OperationName { get; set; }

        public DateTime OperationTime { get; set; }
    }
}
