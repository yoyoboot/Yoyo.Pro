using System;
using System.Collections.Generic;
using Abp.Auditing;
using Abp.Dependency;
using Yoyo.Pro.Auditing;

namespace Yoyo.Pro.Configuration
{
    /// <summary>
    /// YoyoPro 实现的 Auditing 配置器
    /// </summary>
    public interface IYoyoProAuditingConfiguration
    {
        /// <summary>
        /// 审计日志数据类型，默认为<see cref="AuditLogs2"/>
        /// </summary>
        Type AuditLogType { get; }


        /// <summary>
        /// 审计日志仓储类型，默认为<see cref="DefaultAuditLogRepository"/>
        /// </summary>
        Type AuditLogRepositoryType { get; }

        /// <summary>
        /// 扩展数据
        /// </summary>
        Dictionary<string, object> CustomData { get; }

        /// <summary>
        /// 配置使用审计日志仓储类型
        /// </summary>
        /// <typeparam name="TAuditLogRepository">仓储类型</typeparam>
        /// <typeparam name="TAuditLog">审计日志实体类型</typeparam>
        /// <returns></returns>
        IYoyoProAuditingConfiguration UseAuditLogRepositoryType<TAuditLogRepository, TAuditLog>()
            where TAuditLogRepository : IAuditLogRepository<TAuditLog>
            where TAuditLog : AuditLogs2, new();
    }
    public class YoyoProAuditingConfiguration : IYoyoProAuditingConfiguration
    {
        Type _auditLogType = typeof(AuditLogs2);
        Type _auditLogRepositoryType = typeof(DefaultAuditLogRepository<>);

        public YoyoProAuditingConfiguration()
        {
            CustomData = new Dictionary<string, object>();
        }

        public Type AuditLogType => _auditLogType;

        public Type AuditLogRepositoryType => _auditLogRepositoryType;

        public Dictionary<string, object> CustomData { get; }

        public IYoyoProAuditingConfiguration UseAuditLogRepositoryType<TAuditLogRepository, TAuditLog>()
            where TAuditLogRepository : IAuditLogRepository<TAuditLog>
            where TAuditLog : AuditLogs2, new()
        {
            _auditLogType = typeof(TAuditLog);
            _auditLogRepositoryType = typeof(TAuditLogRepository);
            return this;
        }
    }
}
