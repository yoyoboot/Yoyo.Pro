// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yoyo.Pro.EnumHelper
{
    /// <summary>
    /// 枚举的全局变量
    /// </summary>
    public static class EnumConsts
    {
        public static Dictionary<string, List<EnumMap>> BaseEnumMaps { get; }

        static EnumConsts()
        {
            BaseEnumMaps = new Dictionary<string, List<EnumMap>>();
        }
    }
}
