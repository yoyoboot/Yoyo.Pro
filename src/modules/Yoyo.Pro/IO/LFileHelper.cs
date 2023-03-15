using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp;
using Abp.IO;
using JetBrains.Annotations;

namespace Yoyo.Pro.IO
{

    /// <summary>
    ///YoyoPro-PRO 用于文件操作的助手类。
    /// </summary>
    public static class LFileHelper
    {
        /// <summary>
        /// 检查和删除给定的文件，如果它确实存在。
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public static void DeleteIfExists(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        /// <summary>
        /// 获取文件的扩展名。
        /// </summary>
        /// <param name="fileNameWithExtension"></param>
        /// <returns>
        /// 返回不带点的扩展。
        /// 如果给定的参数名称 <paramref name="fileNameWithExtension"></paramref> 不包括点, 则返回 null。
        /// </returns>
        [CanBeNull]
        public static string GetExtension([NotNull] string fileNameWithExtension)
        {
            Check.NotNull(fileNameWithExtension, nameof(fileNameWithExtension));

            var lastDotIndex = fileNameWithExtension.LastIndexOf('.');
            if (lastDotIndex < 0)
            {
                return null;
            }

            return fileNameWithExtension.Substring(lastDotIndex + 1);
        }

        /// <summary>
        /// 打开一个文本文件, 读取该文件的所有行, 然后关闭该文件。
        /// </summary>
        /// <param name="path">要打开以供读取的文件。</param>
        /// <returns>一个字符串, 包含文件的所有行。</returns>
        public static async Task<string> ReadAllTextAsync(string path)
        {
            using (var reader = File.OpenText(path))
            {
                return await reader.ReadToEndAsync();
            }
        }

        /// <summary>
        /// Opens a text file, reads all lines of the file, and then closes the file.
        /// </summary>
        /// <param name="path">The file to open for reading.</param>
        /// <returns>A string containing all lines of the file.</returns>
        public static async Task<byte[]> ReadAllBytesAsync(string path)
        {
            using (var stream = File.Open(path, FileMode.Open))
            {
                var result = new byte[stream.Length];
                await stream.ReadAsync(result, 0, (int)stream.Length);
                return result;
            }
        }

        //TODO: ReadAllLinesAsync

        /// <summary>
        /// 读取文件中的所有行(协程)
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IEnumerable<string> ReadLines(string path)
        {
            using (var fs = new FileStream(path,
                FileMode.Open,
                FileAccess.Read,
                FileShare.ReadWrite,
                0x1000,
                FileOptions.SequentialScan)
                )
            {
                using (var sr = new StreamReader(fs, Encoding.UTF8))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        yield return line;
                    }
                }
            }
        }

        /// <summary>
        /// 删除文件夹中的文件
        /// </summary>
        /// <param name="folderPath">文件夹</param>
        /// <param name="fileNameWithoutExtension">不包含后缀的文件名</param>
        public static void DeleteFilesInFolderIfExists(string folderPath, string fileNameWithoutExtension)
        {
            var directory = new DirectoryInfo(folderPath);
            var tempUserProfileImages = directory
                .GetFiles($"{fileNameWithoutExtension}.*", SearchOption.AllDirectories)
                .ToList();
            foreach (var tempUserProfileImage in tempUserProfileImages)
            {
                FileHelper.DeleteIfExists(tempUserProfileImage.FullName);
            }
        }
    }
}
