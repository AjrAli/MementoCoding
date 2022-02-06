
using SchoolProject.Management.Domain.Common;
using System;

namespace SchoolProject.Management.Domain.Entities
{
    public class Student : AuditableEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Adress { get; set; }

        // Relationships
        public long SchoolId { get; set; }
        public School School { get; set; }
    }
}
