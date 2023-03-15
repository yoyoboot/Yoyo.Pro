// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Abp.Collections.Extensions;

namespace Yoyo.Pro
{
    /// <summary>
    /// 迁移工具配置
    /// </summary>
    public class MigratorOptions
    {
        /// <summary>
        /// 静默迁移，自动退出
        /// </summary>
        public bool QuietMode { get; }

        /// <summary>
        /// 监听模式，迁移完成后监听80端口，docker部署时使用
        /// </summary>
        public bool Listener { get; }


        public MigratorOptions(string[] args)
        {
            if (args.IsNullOrEmpty())
            {
                return;
            }

            foreach (var arg in args)
            {
                switch (arg.ToLower().Trim())
                {
                    case "-q":
                        QuietMode = true;
                        break;
                    case "--listener":
                        Listener = true;
                        break;
                }
            }
        }
    }
}
