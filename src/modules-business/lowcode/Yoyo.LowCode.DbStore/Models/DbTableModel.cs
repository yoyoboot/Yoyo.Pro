namespace Yoyo.LowCode.Models
{
    /// <summary>
    /// 表
    /// </summary>
    public class DbTableModel
    {
        /// <summary>
        /// 标识
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        public string Table { get; set; }

        /// <summary>
        /// 表说明
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 大小
        /// </summary>
        public string Size { get; set; }

        /// <summary>
        /// 总数
        /// </summary>
        public int? Sum { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        public string PrimaryKey { get; set; }

        /// <summary>
        /// 数据源主键
        /// </summary>
        public string DataSourceId { get; set; }
    }
}
