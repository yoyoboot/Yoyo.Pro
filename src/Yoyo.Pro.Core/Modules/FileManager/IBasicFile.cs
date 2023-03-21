// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;

namespace Yoyo.Pro.Modules.FileManager
{
    /// <summary>
    /// 基本文件定义
    /// </summary>
    public interface IBasicFile : IEntity<Guid>
    {
        /// <summary>
        /// 父级 <see cref="IBasicFile" /> Id. 如果是根节点，值为null
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 是否为文件夹
        /// </summary>
        public bool Dir { get; set; }

        /// <summary>
        /// 是否为图片
        /// </summary>
        public bool IsImg { get; set; }

        /// <summary>
        /// 文件的名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 文件原始名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 时间文件夹名称
        /// </summary>
        public string DateDirctoryName { get; set; }

        /// <summary>
        /// 对象类型（阿里云）
        /// </summary>
        public string ObjectType { get; set; }

        /// <summary>
        /// 存储类型（阿里云）
        /// </summary>
        public string StorageClass { get; set; }

        /// <summary>
        /// 修改时间戳（阿里云）
        /// </summary>
        public string TimeModified { get; set; }

        /// <summary>
        /// 文件格式
        /// </summary>
        public string FileExt { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// 格式化的文件大小
        /// </summary>
        public string FormattedSize { get; set; }

        /// <summary>
        /// 大小
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// 宽度
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 高度
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// 是否隐藏
        /// </summary>
        public bool IsHidden { get; set; }

        /// <summary>
        /// 记录文件的层次结构关系
        /// Example: "00001.00042.00005". 这是租户的唯一代码。 当然可以进行修改
        /// </summary>
        public string Code { get; set; }
    }
}
