// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Yoyo.LowCode.UserQueryModule.UserQueryDefinition.Dtos
{
    /// <summary>
    /// 获取编辑信息
    /// </summary>
    public class GetBaseUserQueryForEditOutput
    {
        /// <summary>
        /// 实体Dto
        /// </summary>
        public BaseUserQueryEditDto EntityDto { get; set; }

        public string OperationName { get; set; }

        public DateTime OperationTime { get; set; }
    }
}
