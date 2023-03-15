using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.UI;
using Yoyo.Pro.Dtos;
using Yoyo.Pro.DataFileObjects.DataTempCache;
using Yoyo.Pro.Net.MimeTypes;
using Magicodes.ExporterAndImporter.Excel;
using Magicodes.ExporterAndImporter.Pdf;
using Microsoft.AspNetCore.Hosting;
using Yoyo.Pro.AppFolders;

namespace Yoyo.Pro.DataExporting.Magicodes
{
    public class ExportManager<T> : ITransientDependency
        where T : class, new()
    {
        protected readonly IDataTempFileCacheManager _dataTempFileCacheManager;

        protected readonly IWebHostEnvironment _hostingEnvironment;

        protected readonly IBasicAppFolder _basicAppFolder;

        public virtual IExcelExporter ExcelExporter
        {
            get
            {
                return new ExcelExporter();
            }
        }

        public virtual IPdfExporter PdfExporter
        {
            get
            {
                return new PdfExporter();
            }
        }

        public virtual IBasicAppFolder BasicAppFolder => this._basicAppFolder;

        public ExportManager(IDataTempFileCacheManager dataTempFileCacheManager, IBasicAppFolder basicAppFolder)
        {
            _dataTempFileCacheManager = dataTempFileCacheManager;
            _basicAppFolder = basicAppFolder;
        }


        /// <summary>
        /// 导出Excel
        /// </summary>
        public virtual async Task<FileDto> ExportExcel(string fileName, ICollection<T> dataItems)
        {
            //文件名中如果包含#，导出的文件没有后缀名
            fileName = fileName.Replace("#", "");
            var file = await ExcelExporter.ExportAsByteArray(dataItems);
            var fileDto = new FileDto(fileName, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            Save(file, fileDto);

            return fileDto;
        }


        /// <summary>
        /// 导出Excel表头
        /// </summary>
        public virtual async Task<FileDto> ExportExcelHeader(string fileName, T type)
        {
            //文件名中如果包含#，导出的文件没有后缀名
            fileName = fileName.Replace("#", "");

            var file = await ExcelExporter.ExportHeaderAsByteArray(type);
            var fileDto = new FileDto(fileName, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            Save(file, fileDto);

            return fileDto;
        }

        /// <summary>
        /// 保存方式
        /// </summary>
        /// <param name="data"></param>
        /// <param name="file"></param>
        public virtual void Save(byte[] data, FileDto file)
        {
            _dataTempFileCacheManager.SetFile(file.FileToken, data);
        }
    }
}
