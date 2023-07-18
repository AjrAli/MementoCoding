
using ManagementProject.Domain.Common;
using System.Collections.Generic;

namespace ManagementProject.Domain.Entities
{
    public class School : AuditableEntity
    {
        public School() { }
        public School(long id = 0, string name = "", string adress = "", string town = "", string description = "")
        {
            Id = id;
            Name = name;
            Adress = adress;
            Town = town;
            Description = description;
        }
        public string? Name { get; set; }
        public string? Adress { get; set; }
        public string? Town { get; set; }
        public string? Description { get; set; }

        // Relationships
        public ICollection<Student> Students { get; } = new List<Student>();
    }
}
