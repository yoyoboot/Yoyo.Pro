// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace Yoyo.Pro.PushServices
{
    /// <summary>
    /// 机器人消息通知Web接口
    /// </summary>
    public interface IRobotPushHttpApi : IHttpApi
    {
        /// <summary>
        /// 机器人消息通知HttpPost请求
        /// </summary>
        /// <param name="url">群机器人webhook地址</param>
        /// <param name="data">消息内容</param>
        /// <returns></returns>
        [HttpPost]
        Task<string> MessagePush([Uri] string url, [JsonContent] object data);
    }
}
