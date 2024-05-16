// ReSharper disable once CheckNamespace

namespace Yoyo.LowCode.Authorization
{
    /// <summary>
    ///     数据建模。
    /// </summary>
    public static class DbModelPermissions
    {
        /// <summary>
        ///     BaseDbModel权限节点
        /// </summary>
        public const string BaseDbModel_Node = "Pages.BaseDbModel";

        /// <summary>
        ///     BaseDbModel查询授权
        /// </summary>
        public const string BaseDbModel_Query = BaseDbModel_Node + ".Query";

        /// <summary>
        ///     BaseDbModel创建权限
        /// </summary>
        public const string BaseDbModel_Create = BaseDbModel_Node + ".Create";

        /// <summary>
        ///     BaseDbModel修改权限
        /// </summary>
        public const string BaseDbModel_Edit = BaseDbModel_Node + ".Edit";

        /// <summary>
        ///     BaseDbModel删除权限
        /// </summary>
        public const string BaseDbModel_Delete = BaseDbModel_Node + ".Delete";

        /// <summary>
        ///     BaseDbModel打开数据权限
        /// </summary>
        public const string BaseDbModel_OpenData = BaseDbModel_Node + ".OpenData";

        //// custom codes

        //// custom codes end
    }
}
