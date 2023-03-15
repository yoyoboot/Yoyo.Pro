// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Text;
using System.Text.RegularExpressions;
using Abp.Dependency;
using Abp.Extensions;
using Markdig;
using Yoyo.Pro.Extensions;

namespace Yoyo.Pro.Markdown
{
    /// <summary>
    /// 默认的markdown转换器，基于Markdig实现
    /// </summary>
    public class DefaultMarkdownConverter : IMarkdownConverter, ISingletonDependency
    {

        /// <summary>
        /// markdown文件后缀名
        /// </summary>
        public static string MD_FILE_EXTENSION { get; } = "md";
        /// <summary>
        /// md文件link格式化模板
        /// </summary>
        public static string MD_LINK_FORMAT { get; } = $"[{{0}}](/{MD_FILE_EXTENSION}/{{1}}/{{2}}{{3}}/{{4}})";
        /// <summary>
        /// md文件link匹配正则表达式
        /// </summary>
        public static string MD_LINK_REGEXP { get; } = @$"\[(.*)\]\((.*\.{MD_FILE_EXTENSION})\)";
        /// <summary>
        /// 锚点link匹配正则表达式
        /// </summary>
        public static string ANCHOR_LINK_REGEXP { get; } = @"<a[^>]+href=\""(.*?)\""[^>]*>(.*)?</a>";

        /// <summary>
        /// markdown处理管道
        /// </summary>
        protected readonly MarkdownPipeline _markdownPipeline;

        public DefaultMarkdownConverter()
        {
            _markdownPipeline = new MarkdownPipelineBuilder()
              .UseAdvancedExtensions()
              .Build();
        }

        /// <inheritdoc/>
        public virtual string ConvertToHtml(string markdown)
        {
            //markdown = this.NormalizeLinks(markdown);

            var content = Markdig.Markdown.ToHtml(
                Encoding.UTF8.GetString(Encoding.Default.GetBytes(markdown)),
                _markdownPipeline
                  );

            return content;
        }

        /// <summary>
        /// 对URL路径进行替换和重组
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        protected virtual string NormalizeLinks(string content)
        {
            var normalized = Regex.Replace(content, MD_LINK_REGEXP, (match) =>
            {
                var link = match.Groups[2].Value;
                if (link.IsExternalLink())
                {
                    return match.Value;
                }

                var displayText = match.Groups[1].Value;
                var documentName = RemoveFileExtension(link);

                return string.Format(MD_LINK_FORMAT, displayText, documentName);
            });

            normalized = Regex.Replace(normalized, ANCHOR_LINK_REGEXP, delegate (Match match)
            {
                var link = match.Groups[1].Value;
                if (link.IsExternalLink())
                {
                    return match.Value;
                }

                var displayText = match.Groups[2].Value;
                var documentName = RemoveFileExtension(link);

                return string.Format(MD_LINK_FORMAT, displayText, documentName);
            });

            return normalized;
        }

        /// <summary>
        /// 删除文件后缀
        /// </summary>
        /// <param name="documentName"></param>
        /// <returns></returns>
        protected virtual string RemoveFileExtension(string documentName)
        {
            if (documentName == null)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(documentName))
            {
                return documentName;
            }

            if (!documentName.EndsWith(MD_FILE_EXTENSION, StringComparison.OrdinalIgnoreCase))
            {
                return documentName;
            }

            return documentName.Left(documentName.Length - MD_FILE_EXTENSION.Length - 1);
        }
    }
}
