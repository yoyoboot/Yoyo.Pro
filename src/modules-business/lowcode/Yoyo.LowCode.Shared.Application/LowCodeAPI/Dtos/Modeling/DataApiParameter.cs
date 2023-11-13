// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Yoyo.LowCode.LowCodeAPI.Dtos.Modeling
{
    public class DataApiParameter
    {
        public List<DataApiField> Querys { get; set; }

        public List<DataApiField> Headers { get; set; }

        public object RequestBody { get; set; }

        public object ResponseBody { get; set; }
    }
}
