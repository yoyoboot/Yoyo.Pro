using System;
using System.ComponentModel.DataAnnotations;
using Abp.MultiTenancy;
using Abp.Timing;
using Yoyo.Pro.Users;

namespace Yoyo.Pro.MultiTenancy
{

    /// <summary>
    /// 租户
    /// </summary>
    public class Tenant : AbpTenant<User>
    {
        #region 常量

        /// <summary>
        /// logo类型最大长度
        /// </summary>
        public const int MaxLogoMimeTypeLength = 64;

        #endregion


        #region 字段

        /// <summary>
        /// 订阅结束时间
        /// </summary>
        public DateTime? SubscriptionEndUtc { get; set; }

        /// <summary>
        /// 是否试用
        /// </summary>
        public bool IsInTrialPeriod { get; set; }

        /// <summary>
        /// 自定义cssId
        /// </summary>
        public virtual Guid? CustomCssId { get; set; }

        /// <summary>
        /// 自定义LogoId
        /// </summary>
        public virtual Guid? LogoId { get; set; }

        /// <summary>
        /// Logo文件类型
        /// </summary>
        [MaxLength(MaxLogoMimeTypeLength)]
        public virtual string LogoFileType { get; set; }

        #endregion


        public Tenant()
        {
        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {

        }


        #region 实体函数

        /// <summary>
        /// 是否存在Logo
        /// </summary>
        /// <returns></returns>
        public virtual bool HasLogo()
        {
            return LogoId != null && LogoFileType != null;
        }

        /// <summary>
        /// 清空Logo
        /// </summary>
        public void ClearLogo()
        {
            LogoId = null;
            LogoFileType = null;
        }

        /// <summary>
        /// 是否订阅结束
        /// </summary>
        /// <returns></returns>
        public virtual bool IsSubscriptionEnded()
        {
            return SubscriptionEndUtc < Clock.Now.ToUniversalTime();
        }

        /// <summary>
        /// 剩余天数
        /// </summary>
        /// <returns></returns>
        public virtual int CalculateRemainingDayCount()
        {
            return SubscriptionEndUtc != null ? (SubscriptionEndUtc.Value - Clock.Now.ToUniversalTime()).Days : 0;
        }

        /// <summary>
        /// 是否无限期订阅
        /// </summary>
        /// <returns></returns>
        public virtual bool HasUnlimitedTimeSubscription()
        {
            return SubscriptionEndUtc == null;
        } 

        #endregion
    }
}
