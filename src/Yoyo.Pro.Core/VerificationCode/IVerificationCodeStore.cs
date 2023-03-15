using System.IO;
using Abp.Dependency;

namespace Yoyo.Pro.VerificationCode
{
    /// <summary>
    /// 图形验证码生成服务
    /// </summary>
    public interface IVerificationCodeStore : ISingletonDependency
    {
        //string RndNum(int VcodeNum);
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="code">生成的验证码</param>
        /// <param name="validateCodeType"></param>
        /// <param name="numberLength">生成验证码长度</param>
        /// <returns>文件流</returns>
        MemoryStream Create(out string code, ValidateCodeType validateCodeType = ValidateCodeType.Number, int numberLength = 4);
    }
}
