// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yoyo.Pro.AppFolders
{
    /// <summary>
    /// 应用目录
    /// </summary>
    public interface IBasicAppFolder
    {
        /// <summary>
        /// 获取目录，不存在将抛出异常
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string Get(string key);

        /// <summary>
        /// 获取目录，不存在不会抛出异常
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="path">返回的路径</param>
        /// <returns>存在返回true，不存在返回false</returns>
        bool TryGetValue(string key, out string path);

        /// <summary>
        /// 设置目录
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="path">路径</param>
        /// <param name="createIfNotExists">不存在则创建，默认为false</param>
        void Set(string key, string path, bool createIfNotExists = false);

        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns></returns>
        IReadOnlyDictionary<string, string> GetAll();
    }
}
