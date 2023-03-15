namespace Yoyo.Pro.DataExporting.Magicodes.Dto
{
    public class ExcelImpOutput<T> where T : class, new()
    {
        public ExcelImpDto<T> ExcelDto { get; set; }

        public byte[] FileBytes { get; set; }
    }
}
