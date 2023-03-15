using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Auditing;

namespace Yoyo.Pro.Auditing
{
    public interface IAuditLogRepository<TAuditLog>
        where TAuditLog : AuditLogs2, new()
    {
        IQueryable<TAuditLog> GetAll();

        void Insert(TAuditLog auditLog);

        Task InsertAsync(TAuditLog auditLog);

        Task DeleteAsync(List<TAuditLog> auditLog, DateTime? startTime = null);

        void Delete(List<TAuditLog> auditLog, DateTime? startTime = null);

    }

}
