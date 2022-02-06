using System;


namespace SchoolProject.Management.Domain.Common
{
    public abstract class AuditableEntity
    {
        public long Id { get; private set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTimeOffset LastModifiedDate { get; set; }
    }
}

