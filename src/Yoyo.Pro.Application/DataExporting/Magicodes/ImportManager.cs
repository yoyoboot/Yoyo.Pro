using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Abp.Dependency;
using Yoyo.Pro.DataExporting.Magicodes.Dto;
using Yoyo.Pro.Dtos;
using Yoyo.Pro.DataFileObjects.DataTempCache;
using Yoyo.Pro.Net.MimeTypes;
using Magicodes.ExporterAndImporter.Excel;

namespace Yoyo.Pro.DataExporting.Magicodes
{
    public class ImportManager<T> : ITransientDependency where T : class, new()
    {
        private readonly IDataTempFileCacheManager _dataTempFileCacheManager;
        private IExcelImporter _excelImporter
        {
            get
            {
                return new ExcelImporter();
            }
        }
        public ImportManager(IDataTempFileCacheManager dataTempFileCacheManager)
        {
            _dataTempFileCacheManager = dataTempFileCacheManager;
        }



        /// <summary>
        /// 进行文件导入
        /// </summary>
        /// <param name="fileToken"></param>
        /// <returns></returns>
        public async Task<ExcelImpOutput<T>> ImportExcel(string fileToken)
        {
            var excelImpOutput = new ExcelImpOutput<T>();

            var fileBytes = _dataTempFileCacheManager.GetFile(fileToken);

            using (var stream = new MemoryStream(fileBytes))
            {
                var importResult = await _excelImporter.Import<T>(stream);

                if (importResult.Exception != null)
                {
                    throw importResult.Exception;
                }
                var ret = new ExcelImpDto<T>()
                {
                    RowErrors = importResult.RowErrors,
                    TemplateErrors = importResult.TemplateErrors,
                    HasError = importResult.HasError,
                    Data = importResult.Data,
                };

                excelImpOutput.ExcelDto = ret;
                excelImpOutput.FileBytes = fileBytes;

                return excelImpOutput;
            }

        }

        /// <summary>
        /// 处理标注文件
        /// </summary>
        /// <param name="importResult"></param>
        /// <param name="fileBytes"></param>
        /// <returns></returns>
        public void SaveLabelingErrorFileDto(ExcelImpDto<T> importResult, byte[] fileBytes)
        {
            // 返回错误标注文件的Token
            if (importResult.RowErrors.Count > 0)
            {
                using (var stream = new MemoryStream(fileBytes))
                {
                    _excelImporter.OutputBussinessErrorData<T>(stream, importResult.RowErrors.ToList(), out var fileErrorByte);

                    var fileDto = new FileDto("Error-tagging.xlsx",
                        MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);

                    if (fileErrorByte != null)
                    {
                        Save(fileErrorByte, fileDto);
                        importResult.LabelingErrorFileDto = fileDto;
                    }
                }

            }
        }


        /// <summary>
        /// 生成导入模板文件放入缓存中
        /// </summary>
        /// <returns></returns>
        public async Task<FileDto> GenerateTemplate(string filename)
        {
            var fileByte = await _excelImporter.GenerateTemplateBytes<T>();

            var fileDto = new FileDto(filename,
                MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            Save(fileByte, fileDto);

            return fileDto;
        }


        //保存方式
        protected void Save(byte[] data, FileDto file)
        {
            _dataTempFileCacheManager.SetFile(file.FileToken, data);
        }
    }
}
