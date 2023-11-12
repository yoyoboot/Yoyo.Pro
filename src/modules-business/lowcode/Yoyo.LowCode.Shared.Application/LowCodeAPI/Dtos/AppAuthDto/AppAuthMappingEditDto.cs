// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Yoyo.LowCode.LowCodeAPI.Dtos.AppAuthDto
{
    public class AppAuthMappingEditDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// 乐观锁Token
        /// </summary>
        public string ConcurrencyToken { get; set; }

        /// <summary>
        /// APP授权应用ID
        /// </summary>
        public Guid AppAuthID { get; set; }

        /// <summary>
        /// 接口中心API ID
        /// </summary>
        public Guid AgentID { get; set; }

        /// <summary>
        /// 接口中心API Name
        /// </summary>
        public string AgentName { get; set; }
    }
}
