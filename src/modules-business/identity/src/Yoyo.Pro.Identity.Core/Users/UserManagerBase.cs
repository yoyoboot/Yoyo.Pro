using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Collections.Extensions;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Localization;
using Abp.Organizations;
using Abp.Runtime.Caching;
using Abp.Threading;
using Abp.UI;
using Abp.Zero.Configuration;
using Yoyo.Pro.Roles;
using Yoyo.Pro.Security.PasswordComplexity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Abp.Authorization.Roles;

namespace Yoyo.Pro.Users
{
    /// <summary>
    /// User Manager Base
    /// </summary>
    public abstract class UserManagerBase : AbpUserManager<Role, User>
    {
        protected readonly ISettingManager _settingManager;
        protected readonly IUnitOfWorkManager _unitOfWorkManager;

        public UserManagerBase(
            AbpRoleManager<Role, User> roleManager,
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
            IRepository<UserLogin> userLoginRepository
            )
            : base(
                roleManager,
                store,
                optionsAccessor,
                passwordHasher,
                userValidators,
                passwordValidators,
                keyNormalizer,
                errors,
                services,
                logger,
                permissionManager,
                unitOfWorkManager,
                cacheManager,
                organizationUnitRepository,
                userOrganizationUnitRepository,
                organizationUnitSettings,
                settingManager,
                userLoginRepository)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _settingManager = settingManager;
        }



        #region 数据操作

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public override async Task<IdentityResult> CreateAsync(User user)
        {
            var result = await CheckDuplicateUserAsync(user);
            if (!result.Succeeded)
            {
                return result;
            }

            return await base.CreateAsync(user);
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public override async Task<IdentityResult> UpdateAsync(User user)
        {
            var result = await CheckDuplicateUserAsync(user);
            if (!result.Succeeded)
            {
                return result;
            }

            return await base.UpdateAsync(user);
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        protected override async Task<IdentityResult> UpdateUserAsync(User user)
        {
            var result = await CheckDuplicateUserAsync(user);
            if (!result.Succeeded)
            {
                return result;
            }

            return await base.UpdateUserAsync(user);
        }

        #endregion


        #region 密码操作

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="newPassword">新密码</param>
        /// <param name="tenantId">租户Id</param>
        /// <returns></returns>
        public override async Task<IdentityResult> ChangePasswordAsync(User user, string newPassword, string tenantId)
        {
            await base.InitializeOptionsAsync(tenantId);
            return await base.ChangePasswordAsync(user, newPassword);
        }

        /// <summary>
        /// 获取重置密码的代码
        /// </summary>
        /// <param name="resetPasswrdCode"></param>
        /// <returns></returns>
        public async Task<User> GetUserByResetPasswordCode(string resetPasswrdCode)
        {
            var user = await Users.Where(o => o.PasswordResetCode == resetPasswrdCode)
                .FirstOrDefaultAsync();

            return user;
        }

        /// <summary>
        /// 创建随机密码
        /// </summary>
        /// <returns></returns>
        public async Task<string> CreateRandomPassword()
        {
            var passwordComplexitySetting = new PasswordComplexitySetting
            {
                RequireDigit =
                    await _settingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement
                        .PasswordComplexity.RequireDigit),
                RequireLowercase =
                    await _settingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement
                        .PasswordComplexity.RequireLowercase),
                RequireNonAlphanumeric =
                    await _settingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement
                        .PasswordComplexity.RequireNonAlphanumeric),
                RequireUppercase =
                    await _settingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement
                        .PasswordComplexity.RequireUppercase),
                RequiredLength =
                    await _settingManager.GetSettingValueAsync<int>(AbpZeroSettingNames.UserManagement
                        .PasswordComplexity.RequiredLength)
            };

            var upperCaseLetters = "ABCDEFGHJKLMNOPQRSTUVWXYZ";
            var lowerCaseLetters = "abcdefghijkmnopqrstuvwxyz";
            var digits = "0123456789";
            var nonAlphanumerics = "!@$?_-";

            string[] randomChars = { upperCaseLetters, lowerCaseLetters, digits, nonAlphanumerics };

            var rand = new Random(Environment.TickCount);
            var chars = new List<char>();

            if (passwordComplexitySetting.RequireUppercase)
            {
                chars.Insert(rand.Next(0, chars.Count),
                    upperCaseLetters[rand.Next(0, upperCaseLetters.Length)]);
            }

            if (passwordComplexitySetting.RequireLowercase)
            {
                chars.Insert(rand.Next(0, chars.Count),
                    lowerCaseLetters[rand.Next(0, lowerCaseLetters.Length)]);
            }

            if (passwordComplexitySetting.RequireDigit)
            {
                chars.Insert(rand.Next(0, chars.Count),
                    digits[rand.Next(0, digits.Length)]);
            }

            if (passwordComplexitySetting.RequireNonAlphanumeric)
            {
                chars.Insert(rand.Next(0, chars.Count),
                    nonAlphanumerics[rand.Next(0, nonAlphanumerics.Length)]);
            }

            for (var i = chars.Count; i < passwordComplexitySetting.RequiredLength; i++)
            {
                var rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());
        }

        #endregion


        #region 权限判断


        /// <summary>
        /// 是否具有Admin角色
        /// </summary>
        /// <returns></returns>
        public virtual async Task<bool> IsHaveAdminRole(string userId)
        {
            var role = await RoleManager.GetRoleByNameAsync(Role.AdminRoleName);

            if (role != null)
            {
                var isHaveAdminRole = Users.Where(e => e.Id == userId).Include(e => e.Roles)
                    .Any(e => e.Roles.Any(b => b.RoleId == role.Id));
                return isHaveAdminRole;
            }

            return false;
        }

        #endregion


        #region 查找用户

        /// <summary>
        /// 获取Admin用户
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task<User> GetAdminAsync()
        {
            var user = await FindByNameAsync(AbpUserBase.AdminUserName);

            return user;
        }

        /// <summary>
        /// 查找用户
        /// </summary>
        /// <param name="userIdentifier"></param>
        /// <returns></returns>
        [UnitOfWork]
        public User GetUser(UserIdentifier userIdentifier)
        {
            return AsyncHelper.RunSync(() => GetUserAsync(userIdentifier));
        }

        /// <summary>
        /// 查找用户
        /// </summary>
        /// <param name="userIdentifier"></param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task<User> GetUserOrNullAsync(UserIdentifier userIdentifier)
        {
            using (_unitOfWorkManager.Current.SetTenantId(userIdentifier.TenantId))
            {
                return await FindByIdAsync(userIdentifier.UserId.ToString());
            }
        }



        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="userIdentifier"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task<User> GetUserAsync(UserIdentifier userIdentifier)
        {
            var user = await GetUserOrNullAsync(userIdentifier);
            if (user == null)
            {
                throw new Exception("There is no user: " + userIdentifier);
            }

            return user;
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="userIdentifier"></param>
        /// <returns></returns>
        [UnitOfWork]
        public User GetUserOrNull(UserIdentifier userIdentifier)
        {
            return AsyncHelper.RunSync(() => GetUserOrNullAsync(userIdentifier));
        }

        /// <summary>
        /// 根据邮箱查找用户
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task<User> GetUserByEmail(string emailAddress)
        {
            var user = await Users.Where(o => o.EmailAddress == emailAddress)
                .FirstOrDefaultAsync();

            return user;
        }

        /// <summary>
        /// 根据手机号查找用户
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task<User> GetUserByPhoneNumber(string phoneNumber)
        {
            var user = await Users.Where(o => o.PhoneNumber == phoneNumber)
                .FirstOrDefaultAsync();

            return user;
        }

        /// <summary>
        /// 根据邮箱验证码查找用户
        /// </summary>
        /// <param name="emailConfirmationCode"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task<User> GetUserByEmailConfirmationCode(string emailConfirmationCode)
        {
            var user = await Users.Where(o => o.EmailConfirmationCode == emailConfirmationCode)
                .FirstOrDefaultAsync();

            return user;
        }

        #endregion


        #region 数据检测


        /// <summary>
        /// 检查检查用户数据是否重复
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns>成功</returns>
        /// <exception cref="UserFriendlyException">抛出重复异常</exception>
        public virtual async Task<IdentityResult> CheckDuplicateUserAsync(User user)
        {
            Check.NotNull(user, nameof(user));

            var expectedUserId = user.Id; // 预期用户Id
            var userName = user.UserName; // 用户名
            var emailAddress = user.EmailAddress; // 邮箱地址
            var phoneNumber = user.PhoneNumber; // 手机号

            // 标准化输入数据
            var normalizedUserName = NormalizeName(userName);
            var normalizedEmail = NormalizeEmail(emailAddress);

            // 查找用户
            var duplicateUsers = await Users
                .AsNoTracking()
                .Where(o => o.NormalizedUserName == normalizedUserName
                    || o.NormalizedEmailAddress == normalizedEmail
                    || o.PhoneNumber == phoneNumber
                 )
                .Select(o => new
                {
                    Id = o.Id,
                    NormalizedUserName = o.NormalizedUserName,
                    NormalizedEmailAddress = o.NormalizedEmailAddress,
                    PhoneNumber = o.PhoneNumber,
                    IsDeleted = o.IsDeleted,
                    TenantId = o.TenantId
                })
                .ToListAsync();

            // 重复用户名
            var duplicateUser = duplicateUsers
                .FirstOrDefault(o => o.NormalizedUserName == normalizedUserName);
            if (duplicateUser != null && duplicateUser.Id != expectedUserId)
            {
                throw new UserFriendlyException(string.Format(L("UserAlreadyExists_Msg"), userName));
            }


            // 重复邮箱
            duplicateUser = duplicateUsers
                .FirstOrDefault(o => o.NormalizedEmailAddress == normalizedEmail);
            if (duplicateUser != null && duplicateUser.Id != expectedUserId)
            {
                throw new UserFriendlyException(string.Format(L("EmailAlreadyExists_Msg"), emailAddress));
            }


            // 重复手机号
            duplicateUser = duplicateUsers
                .FirstOrDefault(o => o.PhoneNumber == phoneNumber);
            if (duplicateUser != null && duplicateUser.Id != expectedUserId)
            {
                throw new UserFriendlyException(string.Format(L("PhoneAlreadyExists_Msg"), phoneNumber));
            }


            return IdentityResult.Success;
        }

        /// <summary>
        /// 检查是否有重复的用户名和邮箱
        /// </summary>
        /// <param name="expectedUserId">当前已经存在的UserId</param>
        /// <param name="userName">用户名</param>
        /// <param name="emailAddress">邮箱</param>
        /// <returns></returns>
        [Obsolete("请使用 CheckDuplicateUserAsync 方法")]
        public override async Task<IdentityResult> CheckDuplicateUsernameOrEmailAddressAsync(string expectedUserId,
            string userName, string emailAddress)
        {
            // 这里由 CheckDuplicateUsernameOrEmailAddressOrPhoneNumberAsync 进行检查
            return await ValueTask.FromResult(IdentityResult.Success);
        }


        #endregion


        #region 公共方法

        /// <summary>
        /// 获取当前租户
        /// </summary>
        /// <returns></returns>
        protected virtual string GetCurrentTenantId()
        {
            if (_unitOfWorkManager.Current != null)
            {
                return _unitOfWorkManager.Current.GetTenantId();
            }

            return AbpSession.TenantId;
        }


        #endregion

    }
}
