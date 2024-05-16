using System;
using DatabaseType = SqlSugar.DbType;

namespace Yoyo.LowCode.DbLinks.Dtos
{
    /// <summary>
    /// 数据连接的列表DTO
    /// <see cref="BaseDbLink"/>
    /// </summary>
    public class DbLinkEditDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// 连接名称
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 连接驱动
        /// </summary>
        public DatabaseType DbType { get; set; }

        /// <summary>
        /// 主机名称
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int? Port { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 服务名
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public long? SortCode { get; set; }

        //// custom codes

        //// custom codes end
    }
}
