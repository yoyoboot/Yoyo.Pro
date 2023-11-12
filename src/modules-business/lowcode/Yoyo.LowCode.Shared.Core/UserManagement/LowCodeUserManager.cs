using System;
using System.Collections.Generic;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Organizations;
using Abp.Runtime.Caching;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Yoyo.Pro.Roles;
using Yoyo.Pro.Users;

namespace Yoyo.LowCode.UserManagement
{
    public class LowCodeUserManager : UserManagerBase
    {
        public LowCodeUserManager(AbpRoleManager<Role, User> roleManager,
            AbpUserStore<Role, User> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<User> passwordHasher,
            IEnumerable<IUserValidator<User>> userValidators,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<User>> logger,
            IPermissionManager permissionManager,
            IUnitOfWorkManager unitOfWorkManager,
            ICacheManager cacheManager,
            IRepository<OrganizationUnit> organizationUnitRepository,
            IRepository<UserOrganizationUnit> userOrganizationUnitRepository,
            IOrganizationUnitSettings organizationUnitSettings,
            ISettingManager settingManager,
            IRepository<UserLogin> userLoginRepository) : base(roleManager, store, optionsAccessor, passwordHasher, userValidators, passwordValidators,
            keyNormalizer, errors, services, logger, permissionManager, unitOfWorkManager, cacheManager,
            organizationUnitRepository, userOrganizationUnitRepository, organizationUnitSettings, settingManager, userLoginRepository)
        {
            this.LocalizationSourceName = LowCodeConsts.LocalizationSourceName;
        }
    }
}
