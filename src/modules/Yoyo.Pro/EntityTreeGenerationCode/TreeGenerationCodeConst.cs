namespace Yoyo.Pro.EntityTreeGenerationCode
{
    /// <summary>
    /// 实体生成码的常量
    /// </summary>
    public class TreeGenerationCodeConst
    {

        /// <summary>
        /// Maximum length of the <see cref="DisplayName"/> property.
        /// </summary>
        public const int MaxDisplayNameLength = 128;

        /// <summary>
        /// Maximum depth of an UO hierarchy.
        /// </summary>
        public const int MaxDepth = 16;

        /// <summary>
        ///两个点之间编码的长度
        /// </summary>
        public const int CodeUnitLength = 5;

        /// <summary>
        /// Maximum length of the <see cref="Code"/> property.
        /// </summary>
        public const int MaxCodeLength = MaxDepth * (CodeUnitLength + 1) - 1;
    }
}
