// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Yoyo.LowCode.LowCodeAPI.Dtos.ReponseDataDto
{
    public class ReponseData
    {
        public string code { get; set; }

        public bool success { get; set; }

        public string message { get; set; }

        public object data { get; set; }

        public Exception exception { get; set; }

        public Guid stepBatch { get; set; }

        /// <summary>
        /// 默认为0（0代表返回网关封装的格式 1代表返回实际请求api接口的数据）
        /// </summary>
        public int responseDataType { get; set; }
    }
}
