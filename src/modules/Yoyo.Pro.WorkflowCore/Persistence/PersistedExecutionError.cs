using System;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;

namespace Yoyo.Pro.WorkflowCore.Persistence
{
    public class PersistedExecutionError : Entity<Guid>
    {
        [MaxLength(100)]
        public string WorkflowId { get; set; }

        [MaxLength(100)]
        public string ExecutionPointerId { get; set; }

        public DateTime ErrorTime { get; set; }

        public string Message { get; set; }
    }
}
