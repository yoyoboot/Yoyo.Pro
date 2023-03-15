using System.ComponentModel;

namespace Yoyo.Pro.Roles
{

    /// <summary>
    ///     系统角色默认常量名称
    /// </summary>
    public static class StaticRoleNames
    {
        public static class Host
        {
            public const string Admin = "Admin";
        }

        /// <summary>
        ///     会员角色常量
        /// </summary>
        public class MemberRoleAppConsts
        {

            #region 内置会员等级

            /// <summary>
            ///     青铜会员
            /// </summary>
            [Description("青铜会员")]
            public const string BronzeMemberRoleName = "BronzeMember";
            /// <summary>
            ///     黄金会员
            /// </summary>
            public const string GoldMemberRoleName = "GoldMember";
            /// <summary>
            ///     铂金会员
            /// </summary>
            public const string PlatinumMemberRoleName = "PlatinumMember";
            /// <summary>
            ///     钻石会员
            /// </summary>
            public const string DiamondMemberRoleName = "DiamondMember";
            /// <summary>
            ///     星耀
            /// </summary>
            public const string StarMemberRoleName = "StarMember";
            /// <summary>
            ///     王者
            /// </summary>
            public const string KingMemberRoleName = "KingMember";
            /// <summary>
            ///     荣耀
            /// </summary>
            public const string GloryMemberRoleName = "GloryMember";

            /// <summary>
            ///     合作会员
            /// </summary>
            public const string CooperativeMemberRoleName = "CooperativeMember";


            // 黄金   铂金 钻石 星耀 王者 荣耀 神圣 永恒 至尊

            //Gold, platinum, diamond, star, king, glory, sacred, eternal, supreme

            #endregion
        }



        public static class Tenants
        {
            public const string Admin = "Admin";

            public const string User = "User";
        }
    }
}
