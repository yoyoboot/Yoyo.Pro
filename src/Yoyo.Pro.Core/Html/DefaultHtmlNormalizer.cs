// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.RegularExpressions;
using Abp.Dependency;
using Abp.Extensions;
using Yoyo.Pro.Extensions;

namespace Yoyo.Pro.Html
{
    /// <summary>
    /// 默认实现的Html标准化器
    /// </summary>
    public class DefaultHtmlNormalizer : IHtmlNormalizer, ISingletonDependency
    {
        /// <inheritdoc/>
        public virtual string ReplaceImageSources(string content, string documentRawRootUrl, string localDirectory)
        {
            if (content == null)
            {
                return null;
            }

            content = Regex.Replace(content, @"(<img\s+[^>]*)src=""([^""]*)""([^>]*>)", (match) =>
            {
                if (match.Groups[2].Value.IsExternalLink())
                {
                    return match.Value;
                }

                //本地的图片，需要上传到图床中去

                var newImageSource = documentRawRootUrl.EnsureEndsWith('/') +
                                     (localDirectory.IsNullOrEmpty() ? "" : localDirectory.TrimStart('/').EnsureEndsWith('/')) +
                                     match.Groups[2].Value.TrimStart('/');

                var url = match.Groups[1] + " src=\"" + newImageSource + "\" " + match.Groups[3];
                return url;
            }, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline);

            return content;
        }


        /// <inheritdoc/>
        public virtual string ReplaceCodeBlocksLanguage(string content, string currentLanguage, string newLanguage)
        {
            content = Regex.Replace(
                content,
                "<code class=\"" + currentLanguage + "\">", "<code class=\"" + newLanguage + "\">",
                RegexOptions.IgnoreCase
                );

            return content;
        }

        /// <inheritdoc/>
        public virtual string ReplaceCodeLinkUrl(string content)
        {
            //var content1 =  Regex.Matches(content, "href\\s*=\\s*(?:\"(?<1>[^\"]*)\"|(?<1>\\S+))", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline);
            var linkRx = @"<a\s+(?:[^>]*?\s+)?href=([""])(.*?)\1";
            return Regex.Replace(content, linkRx, (match) =>
            {
                var oldUrl = match.Value;
                //aspnet-core-responsive-scaling-problem.md
                //yoyomooc/aspnet-core-responsive-scaling-problem

                if (oldUrl.Contains(".md"))
                {
                    oldUrl = oldUrl.Replace(".md", "");
                }

                //  oldUrl= oldUrl.Replace(".md", "");

                //yoyomooc/understanding-docker-and-container

                //<a href="understanding-docker-and-container.md"

                //todo:需要它的路由名称拼接而成

                //<a href="understanding-docker-and-container.md"       target="_blank"

                var newUrl = oldUrl + @"       target=""_blank""";
                return newUrl;
            }, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline);
        }

    }
}
