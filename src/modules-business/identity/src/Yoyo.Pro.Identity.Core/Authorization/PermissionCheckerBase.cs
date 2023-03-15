using Abp.Authorization;
using Abp.Authorization.Users;
using Yoyo.Pro.Roles;
using Yoyo.Pro.Users;

namespace Yoyo.Pro.Authorization
{
    public abstract class PermissionCheckerBase : PermissionChecker<Role, User>
    {
        public PermissionCheckerBase(AbpUserManager<Role, User> userManager)
            : base(userManager)
        {
        }
    }
}
