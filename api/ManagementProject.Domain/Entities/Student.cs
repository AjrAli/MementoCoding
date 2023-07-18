
using ManagementProject.Domain.Common;

namespace ManagementProject.Domain.Entities
{
    public class Student : AuditableEntity
    {

        public Student() { }
        public Student(long id = 0, string firstName = "", string lastName = "", int age = 0, string adress = "", long schoolId = 0)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            Adress = adress;
            SchoolId = schoolId;
        }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int Age { get; set; }
        public string? Adress { get; set; }

        // Relationships
        public long SchoolId { get; set; }
        public School? School { get; set; }
    }
}
