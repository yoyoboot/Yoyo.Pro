// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Yoyo.Pro.Modules.FileManager.DomainService
{
    /// <summary>
    /// 基本的文件管理
    /// </summary>
    public interface IBasicFileManager
    {
        /// <summary>
        /// 根据Id查询文件是否存在
        /// </summary>
        /// <returns> </returns>
        Task<bool> IsExistAsync(Guid id);


        /// <summary>
        /// 根据Id查询实体信息
        /// </summary>
        /// <param name="id"> </param>
        /// <returns> </returns>
        Task<IBasicFile> FindBasicFileByIdAsync(Guid id);


        /// <summary>
        /// 添加文件
        /// </summary>
        /// <param name="entity"> 文件实体 </param>
        /// <returns> </returns>
        Task<IBasicFile> CreateBasicFileAsync(IBasicFile entity);

        /// <summary>
        /// 修改文件
        /// </summary>
        /// <param name="entity"> 文件实体 </param>
        /// <returns> </returns>
        Task UpdateBasicFileAsync(IBasicFile entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"> </param>
        /// <returns> </returns>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="input"> Id的集合 </param>
        /// <returns> </returns>
        Task BatchDelete(List<Guid> input);

        /// <summary>
        /// 验证文件是否符合规则
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task ValidateBasicFileAsync(IBasicFile entity);

        /// <summary>
        /// 获取子文件、文件夹
        /// </summary>
        /// <param name="parentId">父级文件夹名称</param>
        /// <param name="recursive">递归查询所有，默认为false</param>
        /// <returns></returns>
        Task<List<IBasicFile>> FindChildrenBasicFileAsync(Guid? parentId, bool recursive = false);

        /// <summary>
        /// 移动文件到指定文件夹
        /// </summary>
        /// <param name="id">文件id</param>
        /// <param name="parentId">文件夹id</param>
        /// <returns> </returns>
        Task MoveAsync(Guid id, Guid? parentId);

        /// <summary>
        /// 处理上传文件信息生成文件数据
        /// </summary>
        /// <param name="file">表单的文件</param>
        /// <param name="isHidden">是否隐藏，默认为false</param>
        /// <param name="isFileType">是否为文件类型，默认为true</param>
        /// <returns></returns>
        Task<IBasicFile> ProcessUploadedBasicFileAsync(IFormFile file, bool isHidden = false, bool isFileType = true);

        /// <summary>
        /// 根据路径获取文件字节
        /// </summary>
        /// <returns></returns>
        Task<byte[]> GetFile(string path);

        /// <summary>
        /// 根据路径获取文件流
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        Task<Stream> GetFileStream(string path);

        /// <summary>
        /// 根据id获取文件字节
        /// </summary>
        /// <returns></returns>
        Task<byte[]> GetFileById(Guid id);

        /// <summary>
        ///  根据id获取文件流
        /// </summary>
        /// <returns></returns>
        Task<Stream> GetFileStreamById(Guid id);

    }
}
