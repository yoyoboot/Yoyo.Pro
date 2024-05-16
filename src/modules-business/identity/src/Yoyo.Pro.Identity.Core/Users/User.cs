using System;
using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Users;
using Abp.Extensions;
using Abp.Timing;

namespace Yoyo.Pro.Users
{
    public class User : AbpUser<User>
    {
        #region 常量

        /// <summary>
        /// 默认密码
        /// </summary>
        public const string DefaultPassword = "bb123456";

        /// <summary>
        /// 手机号最大长度
        /// </summary>
        public new const int MaxPhoneNumberLength = 18;

        /// <summary>
        /// 真实姓名最大长度
        /// </summary>
        public const int MaxRealNameLength = 1024;

        /// <summary>
        /// 工号最大长度
        /// </summary>
        public const int MaxEmployeeNumberLength = 1024;

        #endregion


        #region 字段

        /// <summary>
        /// 登录Token
        /// </summary>
        public virtual string SignInToken { get; set; }

        /// <summary>
        /// 需要修改密码
        /// </summary>
        public virtual bool NeedToChangeThePassword { get; set; }


        /// <summary>
        /// 登录Token过期时间
        /// </summary>
        public virtual DateTime? SignInTokenExpireTimeUtc { get; set; }

        /// <summary>
        /// 邀请码
        /// </summary>
        [MaxLength(256)]
        public virtual string InvitationCode { get; set; }

        /// <summary>
        /// 个人头像Id
        /// </summary>
        public virtual Guid? ProfilePictureId { get; set; }

        //[Obsolete("Name属性已经不在使用，请使用UserName")]
        [Required(AllowEmptyStrings = true)]
        public override string Name { get; set; } = string.Empty;


        //[Obsolete("Surname属性已经不在使用，请使用UserName")]
        [Required(AllowEmptyStrings = true)]
        public override string Surname { get; set; } = string.Empty;

        /// <summary>
        /// 真实姓名
        /// </summary>
        [MaxLength(MaxRealNameLength)]
        public virtual string RealName { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        [MaxLength(MaxEmployeeNumberLength)]
        public virtual string EmployeeNumber { get; set; }

        #endregion


        #region 实体方法

        /// <summary>
        /// 解锁
        /// </summary>
        public void Unlock()
        {
            AccessFailedCount = 0;
            LockoutEndDateUtc = null;
        }

        /// <summary>
        ///     设置令牌过期时间
        /// </summary>
        public void SetSignInToken()
        {
            SignInToken = Guid.NewGuid().ToString();
            SignInTokenExpireTimeUtc = Clock.Now.AddMinutes(1).ToUniversalTime();
        }

        #endregion


        #region 静态方法

        /// <summary>
        /// 创建租户的管理员用户
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="emailAddress"></param>
        /// <param name="adminUserName"></param>
        /// <param name="needToChangeThePassword"></param>
        /// <returns></returns>
        public static User CreateTenantAdminUser(string tenantId, string emailAddress, string adminUserName = null, bool needToChangeThePassword = false)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = adminUserName ?? AdminUserName,
                Name = adminUserName ?? AdminUserName,
                Surname = adminUserName ?? AdminUserName,
                EmailAddress = emailAddress,
                NeedToChangeThePassword = needToChangeThePassword
            };

            user.SetNormalizedNames();

            return user;
        }

        /// <summary>
        /// 创建随机密码
        /// </summary>
        /// <returns></returns>
        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        /// <summary>
        /// 创建随机邮箱
        /// </summary>
        /// <returns></returns>
        public static string CreateRandomEmail()
        {
            return DateTime.Now.Subtract(DateTime.UnixEpoch).TotalMilliseconds.ToString("F0") + "@YoyoPro.com";
        }

        #endregion

    }
}
