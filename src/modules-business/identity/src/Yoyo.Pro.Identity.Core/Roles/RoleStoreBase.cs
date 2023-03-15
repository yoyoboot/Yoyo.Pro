using Abp.Authorization.Roles;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Yoyo.Pro.Users;

namespace Yoyo.Pro.Roles
{
    /// <summary>
    /// Role Store Base
    /// </summary>
    public abstract class RoleStoreBase : AbpRoleStore<Role, User>
    {
        public RoleStoreBase(
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<Role> roleRepository,
            IRepository<RolePermissionSetting> rolePermissionSettingRepository)
            : base(
                unitOfWorkManager,
                roleRepository,
                rolePermissionSettingRepository)
        {
        }
    }
}
