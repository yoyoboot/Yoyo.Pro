using System.Threading.Tasks;

namespace Yoyo.Pro.Security.PasswordComplexity
{
    /// <summary>
    /// 密码复杂度配置Store
    /// </summary>
    public interface IPasswordComplexitySettingStore
    {
        /// <summary>
        /// 获取密码复杂度配置信息
        /// </summary>
        /// <returns></returns>
        Task<PasswordComplexitySetting> GetSettingsAsync();
    }

}
