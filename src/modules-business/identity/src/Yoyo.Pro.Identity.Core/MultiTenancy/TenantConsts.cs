namespace Yoyo.Pro.MultiTenancy
{
    /// <summary>
    /// 租户常量
    /// </summary>
    public class TenantConsts
    {
        public const string TenancyNameRegex = "^[a-zA-Z][a-zA-Z0-9_-]{1,}$";

        public const string DefaultTenantName = "Default";

        public const int MaxNameLength = 128;
    }
}
