// ReSharper disable once CheckNamespace
namespace Yoyo.LowCode.Authorization
{
    /// <summary>
    /// 定义系统的权限名称的字符串常量。
    /// <see cref="UserQueryGroupAuthorizationProvider" />中对权限的定义.
    ///</summary>
    public static class UserQueryGroupPermissions
    {
        /// <summary>
        /// 的权限节点
        ///</summary>
        public const string UserQueryGroup_Node = "Pages.BaseUserQueryGroup.Node";

        /// <summary>
        /// 的查询授权
        ///</summary>
        public const string UserQueryGroup_Query = "Pages.BaseUserQueryGroup.Query";

        /// <summary>
        /// 的创建权限
        ///</summary>
        public const string UserQueryGroup_Create = "Pages.BaseUserQueryGroup.Create";

        /// <summary>
        /// 的修改权限
        ///</summary>
        public const string UserQueryGroup_Edit = "Pages.BaseUserQueryGroup.Edit";

        /// <summary>
        /// 的删除权限
        ///</summary>
        public const string UserQueryGroup_Delete = "Pages.BaseUserQueryGroup.Delete";

        /// <summary>
        /// 复制信息
        ///</summary>
        public const string UserQueryGroup_Copy = "Pages.BaseUserQueryGroup.Copy";

        /// <summary>
        /// 导出Excel
        ///</summary>
        public const string UserQueryGroup_ExportExcel = "Pages.BaseUserQueryGroup.ExportExcel";

        /// <summary>
        /// 从excel文件导入数据
        ///</summary>
        public const string UserQueryGroup_ImportExcel = "Pages.BaseUserQueryGroup.ImportExcel";

        //// custom codes

        //// custom codes end
    }
}
