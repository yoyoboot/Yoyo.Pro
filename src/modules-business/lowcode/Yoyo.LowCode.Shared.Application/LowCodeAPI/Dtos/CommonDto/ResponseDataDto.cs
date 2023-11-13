// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Yoyo.LowCode.LowCodeAPI.Dtos.CommonDto
{
    public class ResponseDataDto
    {
        public int totalpage { get; set; }

        public string skipCount { get; set; }

        public int pageIndex { get; set; }

        public int pageSize { get; set; }

        public object ReponseData { get; set; }
    }
}
