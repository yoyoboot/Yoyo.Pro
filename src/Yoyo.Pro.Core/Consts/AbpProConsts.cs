

namespace Yoyo.Pro.Consts
{

    /// <summary>
    /// 服务于YoyoPro-PRO版本的常量表内容
    /// </summary>
	public class AbpProConsts
	{

		public const int DefaultPageSize = 10;
		public const int MaxPageSize = 1000;


        public const string TokenValidityKey = "token_validity_key";

        public static string UserIdentifier = "user_identifier";

        /// <summary>
        /// 通知系统的常量管理
        /// </summary>
        public static class AppMessage
        {
            /// <summary>
            /// 欢迎语
            /// </summary>
            public const string WelcomeToCms = "App.WelcomeToCms";
            /// <summary>
            /// 发送消息信息
            /// </summary>
            public const string SendMessageAsync = "App.SendMessageAsync";
        }
    }
}
