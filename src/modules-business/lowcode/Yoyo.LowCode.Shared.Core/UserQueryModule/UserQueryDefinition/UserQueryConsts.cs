// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Yoyo.LowCode.UserQueryModule.UserQueryDefinition
{
    /// <summary>
    /// BaseUserQuery 相关常量
    /// </summary>
    public static class UserQueryConsts
    {
        /// <summary>
        /// 名称长度
        /// </summary>
        public const int NameLength = 200;

        /// <summary>
        /// 备注长度
        /// </summary>

        public const int DescriptionLength = 1000;

        /// <summary>
        /// 查询参数的类型
        /// </summary>
        public static class UserQueryParamterType
        {
            /// <summary>
            /// bool
            /// </summary>
            public const string Boolean = nameof(Boolean);

            /// <summary>
            /// 金钱
            /// </summary>
            public const string Decimal = nameof(Decimal);

            /// <summary>
            /// 浮点数
            /// </summary>
            public const string Float = nameof(Float);

            /// <summary>
            /// 长整型
            /// </summary>
            public const string Integer = nameof(Integer);

            /// <summary>
            /// 字符串
            /// </summary>
            public const string String = nameof(String);

            /// <summary>
            /// 时间
            /// </summary>
            public const string Timestamp = nameof(Timestamp);
        }
    }
}
