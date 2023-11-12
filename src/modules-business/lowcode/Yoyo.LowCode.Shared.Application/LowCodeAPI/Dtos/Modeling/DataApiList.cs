// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Abp.AutoMapper;

namespace Yoyo.LowCode.LowCodeAPI.Dtos.Modeling
{
    public class DataApiList
    {
        /// <summary>
        /// api主键
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// api名称
        /// </summary>
        [AutoMapTo()]
        public string ApiName { get; set; }

        /// <summary>
        /// api请求方式
        /// </summary>
        public string ApiMethod { get; set; }

        /// <summary>
        /// api请求路径
        /// </summary>
        public string ApiPath { get; set; }

        /// <summary>
        /// 是否启用安全认证
        /// </summary>
        public string IsSafetySertification { get; set; }

        /// <summary>
        /// 标签名称
        /// </summary>
        public string LableName { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Remarks { get; set; }
    }
}
