using System.Text;
using System.Threading.Tasks;
using Abp.Auditing;
using Abp.Extensions;

namespace Yoyo.Pro.Auditing
{
    public class YoyoProAuditingStore<TAuditLog> : IAuditingStore
      where TAuditLog : AuditLogs2, new()
    {
        readonly IAuditLogRepository<TAuditLog> _auditLogRepository;

        public YoyoProAuditingStore(IAuditLogRepository<TAuditLog> auditLogRepository)
        {
            _auditLogRepository = auditLogRepository;
        }

        public void Save(AuditInfo auditInfo)
        {
            _auditLogRepository.Insert(GetAuditLog(auditInfo));
        }

        public async Task SaveAsync(AuditInfo auditInfo)
        {
            await _auditLogRepository.InsertAsync(GetAuditLog(auditInfo));
        }

        protected virtual TAuditLog GetAuditLog(AuditInfo auditInfo)
        {
            string abpClearException = AuditLog.GetAbpClearException(auditInfo.Exception);
            return new TAuditLog()
            {
                TenantId = auditInfo.TenantId,
                UserId = auditInfo.UserId,
                ServiceName = auditInfo.ServiceName,
                MethodName = auditInfo.MethodName,
                Parameters = auditInfo.Parameters,
                ReturnValue = auditInfo.ReturnValue,
                ExecutionTime = auditInfo.ExecutionTime,
                ExecutionDuration = auditInfo.ExecutionDuration,
                ClientIpAddress = auditInfo.ClientIpAddress,
                ClientName = auditInfo.ClientName,
                BrowserInfo = auditInfo.BrowserInfo,
                Exception = abpClearException,
                ExceptionMessage = auditInfo.Exception?.Message.TruncateWithPostfix(AuditLogs2.MaxExceptionMessageLength),
                ImpersonatorUserId = auditInfo.ImpersonatorUserId,
                ImpersonatorTenantId = auditInfo.ImpersonatorTenantId,
                CustomData = auditInfo.CustomData,
            };
        }
    }

}
