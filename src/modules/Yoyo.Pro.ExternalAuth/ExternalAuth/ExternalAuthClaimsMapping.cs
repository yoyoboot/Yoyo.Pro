namespace Yoyo.Pro.ExternalAuth
{
    public class ExternalAuthClaimsMapping
    {
        /// <summary>
        /// 原始认证数据中的claim
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 扩展登录调用的claim名称
        /// </summary>
        public string Claim { get; set; }
    }
}
