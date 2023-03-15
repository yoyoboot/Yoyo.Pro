using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Roles;
using Yoyo.Pro.Users;

namespace Yoyo.Pro.Roles
{

    public class Role : AbpRole<User>
    {
        /// <summary>
        ///     Admin角色
        /// </summary>
        public const string AdminRoleName = "Admin";

        public const int MaxDescriptionLength = 5000;

        public Role()
        {
        }

        public Role(string tenantId, string displayName)
            : base(tenantId, displayName)
        {
        }

        public Role(string tenantId, string name, string displayName)
            : base(tenantId, name, displayName)
        {
        }

        [StringLength(MaxDescriptionLength)]
        public string Description { get; set; }


        #region 内置角色

        public static class Host
        {
            public const string Admin = "Admin";
        }
        public static class Tenants
        {
            public const string Admin = "Admin";

            public const string User = "User";
        }

        #endregion
    }
}
