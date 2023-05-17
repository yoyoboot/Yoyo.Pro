using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Auditing;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Yoyo.Pro.Auditing
{
    public class DefaultAuditLogRepository<TAuditLog> : IAuditLogRepository<TAuditLog>
      where TAuditLog : AuditLogs2, new()
    {
        readonly IRepository<TAuditLog, string> _auditLogRepository;

        public DefaultAuditLogRepository(IRepository<TAuditLog, string> auditLogRepository)
        {
            _auditLogRepository = auditLogRepository;
        }

        public IQueryable<TAuditLog> GetAll()
        {
            return _auditLogRepository.GetAll().IgnoreQueryFilters();
        }

        public void Insert(TAuditLog auditLog)
        {
            _auditLogRepository.Insert(GetAuditLog(auditLog));
        }

        public async Task InsertAsync(TAuditLog auditLog)
        {
            await _auditLogRepository.InsertAsync(GetAuditLog(auditLog));
        }

        public virtual IRepository<TAuditLog, string> GetTable()
        {
            return this._auditLogRepository;
        }

        public async Task DeleteAsync(List<TAuditLog> auditLog, DateTime? startTime = null)
        {
            await _auditLogRepository.DeleteAsync(x => auditLog.Contains(x));
        }

        public void Delete(List<TAuditLog> auditLog, DateTime? startTime = null)
        {
            _auditLogRepository.Delete(x => auditLog.Contains(x));
        }


        protected virtual TAuditLog GetAuditLog(TAuditLog auditLog)
        {
            auditLog.ServiceName = auditLog.ServiceName.TruncateWithPostfix(AuditLog.MaxServiceNameLength);
            auditLog.MethodName = auditLog.MethodName.TruncateWithPostfix(AuditLog.MaxMethodNameLength);
            auditLog.Parameters = auditLog.Parameters.TruncateWithPostfix(AuditLog.MaxParametersLength);
            auditLog.ReturnValue = auditLog.ReturnValue.TruncateWithPostfix(AuditLog.MaxReturnValueLength);
            auditLog.ClientIpAddress = auditLog.ClientIpAddress.TruncateWithPostfix(AuditLog.MaxClientIpAddressLength);
            auditLog.ClientName = auditLog.ClientName.TruncateWithPostfix(AuditLog.MaxClientNameLength);
            auditLog.BrowserInfo = auditLog.BrowserInfo.TruncateWithPostfix(AuditLog.MaxBrowserInfoLength);
            auditLog.Exception = auditLog.Exception.TruncateWithPostfix(AuditLog.MaxExceptionLength);
            auditLog.CustomData = auditLog.CustomData.TruncateWithPostfix(AuditLog.MaxCustomDataLength);

            return auditLog;
        }
    }

}
