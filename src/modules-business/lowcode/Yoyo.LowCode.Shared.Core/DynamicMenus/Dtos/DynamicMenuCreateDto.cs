// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Yoyo.LowCode.DynamicMenus.Dtos
{
    public class DynamicMenuCreateDto
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int? orderNo { get; set; }

        /// <summary>
        /// 是否隐藏
        /// </summary>
        public bool hideMenu { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string icon { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        public string permission { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public LowCodeDynamicMenuOpenEnum dynamicMenu { get; set; } = LowCodeDynamicMenuOpenEnum.Component;

        /// <summary>
        /// 地址前
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 文件地址
        /// </summary>
        public string path { get; set; }

        /// <summary>
        /// 子集
        /// </summary>
        public List<DynamicMenuCreateDto> Children { get; set; }
    }
}
