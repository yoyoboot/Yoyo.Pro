using System;
using System.ComponentModel.DataAnnotations;

namespace Yoyo.Pro.Dtos
{


    /// <summary>
    /// 文件Dto
    /// </summary>
    public class FileDto
    {
        public FileDto()
        {
        }

        public FileDto(string fileName, string fileType)
        {
            FileName = fileName;
            FileType = fileType;
            FileToken = Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// 文件名称
        /// </summary>
        [Required] public string FileName { get; set; }

        /// <summary>
        /// 文件类型 
        /// </summary>
        [Required] public string FileType { get; set; }

        /// <summary>
        /// 文件的标记
        /// </summary>

        [Required] public string FileToken { get; set; }
    }
}
