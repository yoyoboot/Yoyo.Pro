using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yoyo.Pro.ExternalAuth
{
    /// <summary>
    /// 扩展登录基本配置
    /// </summary>
    public class ExternalAuthOptions
    {
        /// <summary>
        /// 登录调用地址，默认值： /api/ExternalAuth/SignInByProvider
        /// </summary>
        public string SignInPath { get; set; }

        /// <summary>
        /// 注销调用地址，默认值： /api/ExternalAuth/SignOutByProvider
        /// </summary>
        public string SignOutPath { get; set; }

        /// <summary>
        /// 登录回调地址，默认值： /api/ExternalAuth/Verify
        /// </summary>
        public string CallbackPath { get; set; }

        /// <summary>
        /// 登录失败重定向地址，默认值： /account/external-login-errorr
        /// </summary>
        public string SignInFailRedirectUri { get; set; }

        /// <summary>
        /// 登录成功重定向地址，默认值： /account/external-login
        /// </summary>
        public string SignInSuccessRedirectUri { get; set; }

        /// <summary>
        /// 注销登录重定向地址，默认值： /
        /// </summary>
        public string SignOutRedirectUri { get; set; }

        /// <summary>
        /// JWT 安全码，默认值：空
        /// </summary>
        public string SecurityKey { get; set; }

        /// <summary>
        /// JWT 发行者，默认值：external-auth
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// JWT 过期时间，默认值： 30
        /// </summary>
        public int ExpireInSeconds { get; set; }


        public ExternalAuthOptions()
        {
            SignInPath = "/api/ExternalAuth/SignInByProvider";
            SignOutPath = "/api/ExternalAuth/SignOutByProvider";

            CallbackPath = "/api/ExternalAuth/Verify";

            SignInFailRedirectUri = "/account/external-login-errorr";
            SignInSuccessRedirectUri = "/account/external-login";

            SignOutRedirectUri = "/";
        }
    }
}
