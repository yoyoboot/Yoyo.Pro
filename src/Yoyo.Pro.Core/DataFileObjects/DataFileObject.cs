using System;
using System.ComponentModel.DataAnnotations;
using Abp;
using Abp.Domain.Entities;

namespace Yoyo.Pro.DataFileObjects
{
    /// <summary>
    ///     二进制对象
    /// </summary>
    public class DataFileObject : Entity<Guid>, IMayHaveTenant
    {




        public DataFileObject()
        {
            Id = SequentialGuidGenerator.Instance.Create();
        }

        public DataFileObject(string tenantId, byte[] bytes)
            : this()
        {
            TenantId = tenantId;
            Bytes = bytes;
        }




        [Required]
        public virtual byte[] Bytes { get; set; }

        public virtual string TenantId { get; set; }
    }
}
