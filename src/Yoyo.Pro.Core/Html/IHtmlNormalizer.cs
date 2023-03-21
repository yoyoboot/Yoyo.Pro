// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yoyo.Pro.Html
{
    /// <summary>
    /// HTML标准化处理器
    /// </summary>
    public interface IHtmlNormalizer
    {
        /// <summary>
        /// 替换图片的源路径
        /// </summary>
        /// <param name="content"></param>
        /// <param name="documentRawRootUrl"></param>
        /// <param name="localDirectory"></param>
        /// <returns></returns>
        string ReplaceImageSources(string content, string documentRawRootUrl, string localDirectory);

        /// <summary>
        /// 替换代码样式，为 prismJS 进行渲染准备，可以通过这里启动更多内容
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="currentLanguage">旧样式名称</param>
        /// <param name="newLanguage">新样式名称</param>
        /// <returns></returns>
        string ReplaceCodeBlocksLanguage(string content, string currentLanguage, string newLanguage);


        /// <summary>
        /// 为每个A标签 增加target=""_blank""
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        string ReplaceCodeLinkUrl(string content);
    }
}
