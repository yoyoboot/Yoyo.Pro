using System.ComponentModel;

namespace Yoyo.Pro.VerificationCode
{
    /// <summary>
    ///     验证码类型
    /// </summary>
    public enum ValidateCodeType : byte
    {
        /// <summary>
        ///纯数字
        /// </summary>
        [Description("纯数字")]
        Number = 0,
        /// <summary>
        /// 纯字母
        /// </summary>
        [Description("纯字母")]
        English = 1,
        /// <summary>
        /// 数值与字母的组合
        /// </summary>
        [Description("数值与字母的组合")]
        NumberAndLetter = 2,
        /// <summary>
        /// 汉字
        /// </summary>
        [Description("汉字")]
        Hanzi = 3
    }
}
