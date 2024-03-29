using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Organizations;
using Yoyo.Pro.Roles;

namespace Yoyo.Pro.Users
{
    /// <summary>
    /// User Store Base
    /// </summary>
    public abstract class UserStoreBase : AbpUserStore<Role, User>
    {
        public UserStoreBase(
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<User> userRepository,
            IRepository<Role> roleRepository,
            IRepository<UserRole> userRoleRepository,
            IRepository<UserLogin> userLoginRepository,
            IRepository<UserClaim> userClaimRepository,
            IRepository<UserPermissionSetting> userPermissionSettingRepository,
            IRepository<UserOrganizationUnit> userOrganizationUnitRepository,
            IRepository<OrganizationUnitRole> organizationUnitRoleRepository,
            IRepository<UserToken, string> userTokenRepository
            )
            : base(
                unitOfWorkManager,
                userRepository,
                roleRepository,
                userRoleRepository,
                userLoginRepository,
                userClaimRepository,
                userPermissionSettingRepository,
                userOrganizationUnitRepository,
                organizationUnitRoleRepository,
                userTokenRepository
                )
        {
        }
    }
}
