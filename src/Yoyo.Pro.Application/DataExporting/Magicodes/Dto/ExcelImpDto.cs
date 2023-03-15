using System.Collections.Generic;
using Yoyo.Pro.Dtos;
using Magicodes.ExporterAndImporter.Core.Models;

namespace Yoyo.Pro.DataExporting.Magicodes.Dto
{
    public class ExcelImpDto<T> where T : class
    {    
        
        /// <summary>导入数据</summary>
        public virtual ICollection<T> Data { get; set; }
        /// <summary>模板错误</summary>
        public virtual IList<TemplateErrorInfo> TemplateErrors { get; set; }
        /// <summary>验证错误</summary>
        public virtual IList<DataRowErrorInfo> RowErrors { get; set; }
        /// <summary>是否存在导入错误</summary>
        public virtual bool HasError { get; set; }

        /// <summary>
        /// 错误标注文件的Token
        /// </summary>
        public FileDto LabelingErrorFileDto { get; set; }
    }
}
