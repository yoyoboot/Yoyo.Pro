using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Abp.Collections.Extensions;
using Abp.Domain.Entities.Auditing;
using Abp.Extensions;

namespace Yoyo.LowCode.DynamicMenus
{
    /// <summary>
    ///     动态菜单
    /// </summary>
    public class BaseDynamicMenu : FullAuditedEntity<Guid>
    {
        /// <summary>
        ///     Length of a code unit between dots.
        /// </summary>
        public const int CodeUnitLength = 5;

        /// <summary>
        ///     是否是系统菜单
        /// </summary>
        public bool IsSystem { get; set; }

        /// <summary>
        /// 组件名称
        /// </summary>
        [MaxLength(BaseDynamicMenuConsts.TextMaxCount)]
        public string Name { get; set; }

        /// <summary>
        ///     父级Id
        /// </summary>
        //[MaxLength(LowCodeConsts.GuidMaxCount)]
        public Guid? ParentId { get; set; }

        /// <summary>
        ///     编码  (记录文件的层次结构关系)
        ///     Example: "00001.00042.00005". 这是租户的唯一代码。 当然可以进行修改
        /// </summary>
        [Required]
        [MaxLength(BaseDynamicMenuConsts.CodeMaxCount)]
        public virtual string Code { get; set; }

        /// <summary>
        ///     名称
        /// </summary>
        [MaxLength(BaseDynamicMenuConsts.TextMaxCount)]
        public string Text { get; set; }

        /// <summary>
        /// 权限编码
        /// </summary>
        public string PermissionCode { get; set; }

        /// <summary>
        ///     图标
        /// </summary>
        [MaxLength(BaseDynamicMenuConsts.IconMaxCount)]
        public string Icon { get; set; }

        /// <summary>
        ///     地址
        /// </summary>
        [MaxLength(BaseDynamicMenuConsts.LinkMaxCount)]
        public string Link { get; set; }

        /// <summary>
        /// 模板路径
        /// </summary>
        public string TemplatePath { get; set; }

        /// <summary>
        ///     打开方式
        /// </summary>
        public LowCodeDynamicMenuOpenEnum OpenEnum { get; set; }

        /// <summary>
        ///     是否可见（Y-是，N-否）
        /// </summary>
        public bool Hide { get; set; }

        /// <summary>
        ///     排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        ///     备注
        /// </summary>
        [MaxLength(LowCodeConsts.RemarkMaxCount)]
        public string Remark { get; set; }

        /// <summary>
        ///     将子代码附加到父代码。 例如:如果parentCode = "00001"，则childCode = "00042"，然后返回"00001.00042"。
        /// </summary>
        /// <param name="parentCode"> 父类的代码。 如果父节点是根节点，则可以为空或空。 </param>
        /// <param name="childCode"> 子代码. </param>
        public static string AppendCode(string parentCode, string childCode)
        {
            if (childCode.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(childCode), "子代码不能为空或者为null");
            }

            if (parentCode.IsNullOrEmpty())
            {
                return childCode;
            }

            return parentCode + "." + childCode;
        }

        /// <summary>
        ///     为给定的数字创建代码。
        ///     Example: if numbers are 4,2 then returns "00004.00002";
        /// </summary>
        /// <param name="numbers"> Numbers </param>
        public static string CreateCode(params int[] numbers)
        {
            if (numbers.IsNullOrEmpty())
            {
                return null;
            }

            return numbers.Select(number => number.ToString(new string('0', CodeUnitLength))).JoinAsString(".");
        }

        /// <summary>
        ///     计算给定代码的下一个代码。
        ///     Example: if code = "00019.00055.00001" 返回 "00019.00055.00002".
        /// </summary>
        /// <param name="code"> The code. </param>
        public static string CalculateNextCode(string code)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
            }

            var parentCode = GetParentCode(code);
            var lastUnitCode = GetLastUnitCode(code);

            return AppendCode(parentCode, CreateCode(Convert.ToInt32(lastUnitCode) + 1));
        }

        /// <summary>
        ///     Gets parent code.
        ///     Example: if code = "00019.00055.00001" returns "00019.00055".
        /// </summary>
        /// <param name="code"> The code. </param>
        public static string GetParentCode(string code)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
            }

            var splittedCode = code.Split('.');
            if (splittedCode.Length == 1)
            {
                return null;
            }

            return splittedCode.Take(splittedCode.Length - 1).JoinAsString(".");
        }

        /// <summary>
        ///     Gets the last unit code.
        ///     Example: if code = "00019.00055.00001" returns "00001".
        /// </summary>
        /// <param name="code"> The code. </param>
        public static string GetLastUnitCode(string code)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
            }

            var splittedCode = code.Split('.');
            return splittedCode[splittedCode.Length - 1];
        }

        /// <summary>
        ///     Gets relative code to the parent.
        ///     Example: if code = "00019.00055.00001" and parentCode = "00019" then returns "00055.00001".
        /// </summary>
        /// <param name="code"> The code. </param>
        /// <param name="parentCode"> The parent code. </param>
        public static string GetRelativeCode(string code, string parentCode)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
            }

            if (parentCode.IsNullOrEmpty())
            {
                return code;
            }

            if (code.Length == parentCode.Length)
            {
                return null;
            }

            return code.Substring(parentCode.Length + 1);
        }
    }
}
