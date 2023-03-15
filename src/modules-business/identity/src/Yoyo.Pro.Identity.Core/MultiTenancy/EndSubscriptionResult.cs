namespace Yoyo.Pro.MultiTenancy
{
    /// <summary>
    /// 结束订阅类型
    /// </summary>
    public enum EndSubscriptionResult : byte
    {
        /// <summary>
        /// 设置为不活跃
        /// </summary>
        TenantSetInActive = 0,
        /// <summary>
        /// 切换版本
        /// </summary>
        AssignedToAnotherEdition = 1
    }
}
