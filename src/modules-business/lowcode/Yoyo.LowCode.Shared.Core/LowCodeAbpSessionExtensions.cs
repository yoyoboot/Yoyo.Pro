using Abp.Runtime.Session;
using Yoyo.Pro.Runtime.Session;

namespace Yoyo.LowCode
{
    public static class LowCodeAbpSessionExtensions
    {
        /// <summary>
        /// 获取session中的用户名，需要AbpSesssion实现<see cref="IHasUserName"/>接口
        /// </summary>
        /// <param name="abpSession"></param>
        /// <returns></returns>
        public static string GetUserName(this IAbpSession abpSession)
        {
            if (abpSession is IHasUserName hasUserName)
            {
                return hasUserName.UserName;
            }

            return string.Empty;
        }
    }
}
