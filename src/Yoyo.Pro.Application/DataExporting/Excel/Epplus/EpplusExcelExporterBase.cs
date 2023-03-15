using System;
using System.Collections.Generic;
using Castle.Core.Internal;
using Yoyo.Pro.Dtos;
using Yoyo.Pro.DataFileObjects.DataTempCache;
using Yoyo.Pro.Net.MimeTypes;
using OfficeOpenXml;
using Abp;
using Abp.Runtime.Session;
using Abp.Dependency;

namespace Yoyo.Pro.DataExporting.Excel.Epplus
{
    public abstract class EpplusExcelExporterBase : AbpServiceBase, ITransientDependency
    {
        /// <summary>
        /// Gets current session information.
        /// </summary>
        public virtual IAbpSession AbpSession { get; set; }

        protected readonly IDataTempFileCacheManager _dataTempFileCacheManager;

        public EpplusExcelExporterBase(IDataTempFileCacheManager dataTempFileCacheManager, string localizationSourceName = null)
        {
            _dataTempFileCacheManager = dataTempFileCacheManager;
            LocalizationSourceName = localizationSourceName ?? YoyoProConsts.LocalizationSourceName;

            AbpSession = NullAbpSession.Instance;
        }
        /// <summary>
        /// 创建Excel包
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="createor"></param>
        /// <returns></returns>
        protected virtual FileDto CreateExcelPackage(string fileName, Action<ExcelPackage> createor)
        {
            var file = new FileDto(fileName, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);

            using (var excelPackage = new ExcelPackage())
            {
                createor(excelPackage);
                Save(excelPackage, file);
            }

            return file;
        }

        /// <summary>
        /// 创建Excel表的头
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="headerTexts"></param>
        protected virtual void AddHeader(ExcelWorksheet sheet, params string[] headerTexts)
        {
            if (headerTexts.IsNullOrEmpty())
            {
                return;
            }


            for (int i = 0; i < headerTexts.Length; i++)
            {
                AddHeader(sheet, i + 1, headerTexts[i]);
            }


        }

        /// <summary>
        /// 创建Excel表的头
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="columnIndex"></param>
        /// <param name="headerText"></param>
        protected virtual void AddHeader(ExcelWorksheet sheet, int columnIndex, string headerText)
        {

            sheet.Cells[1, columnIndex].Value = headerText;
            sheet.Cells[1, columnIndex].Style.Font.Bold = true;
        }

        /// <summary>
        /// 添加内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sheet"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="items"></param>
        /// <param name="propertySelectors"></param>
        protected virtual void AddObject<T>(ExcelWorksheet sheet, int startRowIndex, IList<T> items,
            params Func<T, object>[] propertySelectors)
        {
            if (items.IsNullOrEmpty() || propertySelectors.IsNullOrEmpty())
            {
                return;
            }



            for (int i = 0; i < items.Count; i++)
            {
                for (int j = 0; j < propertySelectors.Length; j++)
                {
                    sheet.Cells[i + startRowIndex, j + 1].Value = propertySelectors[j](items[i]);
                }
            }


        }

        /// <summary>
        /// 保存到文件
        /// </summary>
        /// <param name="excelPackage"></param>
        /// <param name="file"></param>
        protected virtual void Save(ExcelPackage excelPackage, FileDto file)
        {
            _dataTempFileCacheManager.SetFile(file.FileToken, excelPackage.GetAsByteArray());
        }
    }
}
