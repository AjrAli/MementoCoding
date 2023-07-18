using System;


namespace ManagementProject.Domain.Common
{
    public abstract class AuditableEntity
    {
        public long Id { get; protected set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTimeOffset LastModifiedDate { get; set; }
    }
}

