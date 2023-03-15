using System;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Yoyo.Pro.WorkflowCore.Persistence
{
    public class PersistedEvent : CreationAuditedEntity<Guid>, IMayHaveTenant
    {
        [MaxLength(200)]
        public string EventName { get; set; }

        [MaxLength(200)]
        public string EventKey { get; set; }

        public string EventData { get; set; }

        public DateTime EventTime { get; set; }
        public bool IsProcessed { get; set; }
        public string TenantId { get; set; }
    }
}
