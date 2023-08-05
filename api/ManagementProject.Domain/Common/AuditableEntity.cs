using System;
using System.ComponentModel.DataAnnotations;

namespace ManagementProject.Domain.Common
{
    public abstract class AuditableEntity
    {
        [Key]
        public long Id { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTimeOffset LastModifiedDate { get; set; }
    }
}

