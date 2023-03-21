// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Web.Models;

namespace Yoyo.Pro.Dtos
{
    /// <summary>
    /// 上传文件输出结果dto
    /// </summary>
    public class UploadFileOutputDto : ErrorInfo
    {
        public UploadFileOutputDto()
        {
        }

        public UploadFileOutputDto(ErrorInfo error)
        {
            Code = error.Code;
            Details = error.Details;
            Message = error.Message;
            ValidationErrors = error.ValidationErrors;
        }

        /// <summary>
        /// 文件Id
        /// </summary>
        public Guid? FileId { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
    }
}
