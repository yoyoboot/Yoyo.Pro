// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Yoyo.LowCode
{
    public static class LowCodeConfigs
    {
        /// <summary>
        /// 本地化配置
        /// </summary>
        public static class Localization
        {
            /// <summary>
            /// 源名称，默认值为 YoyoLowCode
            /// </summary>
            public static string SourceName { get; set; } = "YoyoLowCode";
        }

        /// <summary>
        /// EFCore配置
        /// </summary>
        public static class Database
        {
            /// <summary>
            /// 跳过DbContext注册
            /// </summary>
            public static bool SkipDbContextRegistration { get; set; }

            /// <summary>
            /// 跳过种子数据
            /// </summary>
            public static bool SkipDbSeed { get; set; }
        }

        /// <summary>
        /// 权限配置
        /// </summary>
        public static class Authorization
        {
            /// <summary>
            /// 跳过权限
            /// </summary>
            public static bool SkipAuthorization { get; set; }
        }

        public static class Dto
        {
            /// <summary>
            /// 默认 按照创建时间倒序
            /// </summary>
            public static string DefaultSortByCreationTimeDesc { get; } = "CreationTime desc";

            /// <summary>
            /// NdoDto 按照名称排序
            /// </summary>
            public static string NdoSortByName { get; } = "Name asc";

            /// <summary>
            /// RdoBase 按照名称排序 建议直接医用上面的那个
            /// </summary>
            public static string RdoBaseSortByName { get; } = "Name asc";
        }
    }
}
