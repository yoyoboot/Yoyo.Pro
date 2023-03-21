// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yoyo.Pro.Markdown
{
    /// <summary>
    /// Markdown转换器
    /// </summary>
    public interface IMarkdownConverter
    {
        /// <summary>
        /// 将markdown转换为html
        /// </summary>
        /// <param name="markdown"></param>
        /// <returns></returns>
        string ConvertToHtml(string markdown);
    }
}
