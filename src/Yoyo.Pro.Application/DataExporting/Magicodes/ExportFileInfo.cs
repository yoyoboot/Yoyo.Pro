using System;

namespace Yoyo.Pro.DataExporting.Magicodes
{
    public class ExportFileInfo
    {
        public string FileName { get; set; }

        public string FileType { get; set; }

        public string FileToken { get; set; }

        public ExportFileInfo()
        {
        }

        public ExportFileInfo(string fileName, string fileType)
        {
            this.FileName = fileName;
            this.FileType = fileType;
            this.FileToken = Guid.NewGuid().ToString("N");
        }
    }
}
