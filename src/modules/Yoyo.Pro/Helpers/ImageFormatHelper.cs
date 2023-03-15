using System.DrawingCore;
using System.DrawingCore.Imaging;
using System.IO;

namespace Yoyo.Pro.Helpers
{

    public class ImageFormatHelper
    {
        /// <summary>
        /// 字节转图片格式
        /// </summary>
        /// <param name="fileBytes"></param>
        /// <returns></returns>
        public static ImageFormat GetRawImageFormat(byte[] fileBytes)
        {
            using (var ms = new MemoryStream(fileBytes))
            {
                var fileImage = Image.FromStream(ms);
                return fileImage.RawFormat;
            }
        }
    }
}

