// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Yoyo.LowCode.DataDictionarys.Authorization
{
    /// <summary>
    /// 定义系统的权限名称的字符串常量。
    /// <see cref="BaseDictionaryAuthorizationProvider" />中对权限的定义.
    ///</summary>
    public class BaseDictionaryPermissions
    {
        /// <summary>
        /// BaseDictionary权限节点
        ///</summary>
        public const string BaseDictionary_Node = "Pages.BaseDictionary";

        /// <summary>
        /// BaseDictionary查询授权
        ///</summary>
        public const string BaseDictionary_Query = BaseDictionary_Node + ".Query";

        /// <summary>
        /// BaseDictionary创建权限
        ///</summary>
        public const string BaseDictionary_Create = BaseDictionary_Node + ".Create";

        /// <summary>
        /// BaseDictionary修改权限
        ///</summary>
        public const string BaseDictionary_Edit = BaseDictionary_Node + ".Edit";

        /// <summary>
        /// BaseDictionary删除权限
        ///</summary>
        public const string BaseDictionary_Delete = BaseDictionary_Node + ".Delete";

        /// <summary>
		/// BaseDictionary批量删除权限
		///</summary>
		public const string BaseDictionary_BatchDelete = BaseDictionary_Node + ".BatchDelete";

        /// <summary>
        /// BaseDictionary导出Excel
        ///</summary>
        //public const string BaseDictionary_ExportExcel = BaseDictionary_Node + ".ExportExcel";

        //// custom codes

        //// custom codes end
    }
}
