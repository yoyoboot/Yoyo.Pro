// ReSharper disable once CheckNamespace

namespace Yoyo.LowCode.Authorization
{
    /// <summary>
    ///     定义系统的权限名称的字符串常量。
    ///     <see cref="DbLinkAuthorizationProvider" />中对权限的定义.
    /// </summary>
    public static class DbLinkPermissions
    {
        /// <summary>
        ///     数据中心
        /// </summary>
        public const string BaseDbCenter_Node = "Pages.BaseDbCenter_Node";

        /// <summary>
        ///     BaseDbLink权限节点
        /// </summary>
        public const string BaseDbLink_Node = "Pages.BaseDbLink";

        /// <summary>
        ///     BaseDbLink查询授权
        /// </summary>
        public const string BaseDbLink_Query = BaseDbLink_Node + ".Query";

        /// <summary>
        ///     BaseDbLink创建权限
        /// </summary>
        public const string BaseDbLink_Create = BaseDbLink_Node + ".Create";

        /// <summary>
        ///     BaseDbLink修改权限
        /// </summary>
        public const string BaseDbLink_Edit = BaseDbLink_Node + ".Edit";

        /// <summary>
        ///     BaseDbLink删除权限
        /// </summary>
        public const string BaseDbLink_Delete = BaseDbLink_Node + ".Delete";

        /// <summary>
        ///     BaseDbLink批量删除权限
        /// </summary>
        public const string BaseDbLink_BatchDelete = BaseDbLink_Node + ".BatchDelete";

        /// <summary>
        ///     BaseDbLink导出Excel
        /// </summary>
        public const string BaseDbLink_ExportExcel = BaseDbLink_Node + ".ExportExcel";

        //// custom codes

        //// custom codes end
    }
}
