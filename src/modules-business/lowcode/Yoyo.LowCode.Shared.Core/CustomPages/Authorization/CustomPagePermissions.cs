// ReSharper disable once CheckNamespace
namespace Yoyo.LowCode.Authorization
{
    /// <summary>
    /// 定义系统的权限名称的字符串常量。
    /// <see cref="CustomPageAuthorizationProvider" />中对权限的定义.
    ///</summary>
    public static class CustomPagePermissions
    {
        /// <summary>
        /// BaseCustomPage权限节点
        ///</summary>
        public const string BaseCustomPage_Node = "Pages.BaseCustomPage";

        /// <summary>
        /// BaseCustomPage查询授权
        ///</summary>
        public const string BaseCustomPage_Query = BaseCustomPage_Node + ".Query";

        /// <summary>
        /// BaseCustomPage创建权限
        ///</summary>
        public const string BaseCustomPage_Create = BaseCustomPage_Node + ".Create";

        /// <summary>
        /// BaseCustomPage修改权限
        ///</summary>
        public const string BaseCustomPage_Edit = BaseCustomPage_Node + ".Edit";

        /// <summary>
        /// BaseCustomPage删除权限
        ///</summary>
        public const string BaseCustomPage_Delete = BaseCustomPage_Node + ".Delete";

        /// <summary>
		/// BaseCustomPage批量删除权限
		///</summary>
		public const string BaseCustomPage_BatchDelete = BaseCustomPage_Node + ".BatchDelete";

        /// <summary>
        /// BaseCustomPage导出Excel
        ///</summary>
        public const string BaseCustomPage_ExportExcel = BaseCustomPage_Node + ".ExportExcel";

        //// custom codes

        //// custom codes end
    }
}
