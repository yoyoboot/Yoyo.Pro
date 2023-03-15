using System.Security.Claims;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Zero.Configuration;
using Yoyo.Pro.MultiTenancy.Tenants;
using Yoyo.Pro.Roles;
using Yoyo.Pro.Users;
using Microsoft.AspNetCore.Identity;
using Yoyo.Pro.MultiTenancy;
using Abp.Authorization.Roles;

namespace Yoyo.Pro.Authorization
{
    public abstract class LogInManagerBase : AbpLogInManager<Tenant, Role, User>
    {
        protected readonly UserClaimsPrincipalFactory _claimsPrincipalFactory;

        public LogInManagerBase(
            AbpUserManager<Role, User> userManager,
            IMultiTenancyConfig multiTenancyConfig,
            IRepository<Tenant> tenantRepository,
            IUnitOfWorkManager unitOfWorkManager,
            ISettingManager settingManager,
            IRepository<UserLoginAttempt> userLoginAttemptRepository,
            IUserManagementConfig userManagementConfig,
            IIocResolver iocResolver,
            IPasswordHasher<User> passwordHasher,
            AbpRoleManager<Role, User> roleManager,
            UserClaimsPrincipalFactory claimsPrincipalFactory)
            : base(
                  userManager,
                  multiTenancyConfig,
                  tenantRepository,
                  unitOfWorkManager,
                  settingManager,
                  userLoginAttemptRepository,
                  userManagementConfig,
                  iocResolver,
                  passwordHasher,
                  roleManager,
                  claimsPrincipalFactory)
        {
            _claimsPrincipalFactory = claimsPrincipalFactory;
        }

        /// <summary>
        /// 直接根据用户信息登陆
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns>用户身份信息</returns>
        public virtual async Task<ClaimsIdentity> LoginAsync(User user)
        {
            ClaimsIdentity identity = (ClaimsIdentity)(await _claimsPrincipalFactory.CreateAsync(user)).Identity;
            return identity;
        }




    }
}
