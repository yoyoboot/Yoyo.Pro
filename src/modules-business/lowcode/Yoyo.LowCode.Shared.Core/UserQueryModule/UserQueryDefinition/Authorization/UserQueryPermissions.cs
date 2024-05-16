// ReSharper disable once CheckNamespace
namespace Yoyo.LowCode.Authorization
{
    /// <summary>
    /// 定义系统的权限名称的字符串常量。
    /// <see cref="UserQueryAuthorizationProvider" />中对权限的定义.
    ///</summary>
    public static class UserQueryPermissions
    {
        /// <summary>
        /// 的权限节点
        ///</summary>
        public const string UserQuery_Node = "Pages.BaseUserQuery.Node";

        /// <summary>
        /// 的查询授权
        ///</summary>
        public const string UserQuery_Query = "Pages.BaseUserQuery.Query";

        /// <summary>
        /// 的创建权限
        ///</summary>
        public const string UserQuery_Create = "Pages.BaseUserQuery.Create";

        /// <summary>
        /// 的修改权限
        ///</summary>
        public const string UserQuery_Edit = "Pages.BaseUserQuery.Edit";

        /// <summary>
        /// 的删除权限
        ///</summary>
        public const string UserQuery_Delete = "Pages.BaseUserQuery.Delete";

        /// <summary>
        /// 复制信息
        ///</summary>
        public const string UserQuery_Copy = "Pages.BaseUserQuery.Copy";

        /// <summary>
        /// 导出Excel
        ///</summary>
        public const string UserQuery_ExportExcel = "Pages.BaseUserQuery.ExportExcel";

        /// <summary>
        /// 从excel文件导入数据
        ///</summary>
        public const string UserQuery_ImportExcel = "Pages.BaseUserQuery.ImportExcel";

        //// custom codes

        //// custom codes end
    }
}
