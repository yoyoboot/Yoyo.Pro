namespace Yoyo.LowCode.Models
{
    /// <summary>
    /// 字段
    /// </summary>
    public class DbTableFieldModel
    {
        /// <summary>
        /// 字段名
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 字段说明
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// 数据长度
        /// </summary>
        public string DataLength { get; set; }

        public DataTypeEnum DataTypeEnum { get; set; }

        /// <summary>
        /// 自增
        /// </summary>
        public string Identity { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        public int? PrimaryKey { get; set; }

        /// <summary>
        /// 允许null值
        /// </summary>
        public int? AllowNull { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public string Defaults { get; set; }
    }
}
