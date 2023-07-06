using SchoolProject.Management.Domain.Entities;
using SchoolProject.Management.Persistence.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolProject.Management.Persistence.Seed
{
    public static class DatabaseSeeder
    {
        public async static Task SeedAsync(SchoolManagementDbContext context)
        {
            SeedSchools(context);
            SeedStudents(context);
            await context.SaveChangesAsync();
        }

        private async static void SeedSchools(SchoolManagementDbContext context)
        {
            if (context.Schools != null && !context.Schools.Any())
            {
                var schools = new List<School>
                {
                    new School(1, "test", "adres", "town", "desc"),
                    new School(2, "test2", "adres2", "town2", "desc2")
                };

                await context.Schools.AddRangeAsync(schools);
            }
        }

        private async static void SeedStudents(SchoolManagementDbContext context)
        {
            if (context.Students != null && !context.Students.Any())
            {
                var students = new List<Student>
                {
                    new Student(1, "Test", "Test", 10, "MyAdress", 1),
                    new Student(2, "Test2", "Test2", 15, "MyAdress2", 2)
                };

                await context.Students.AddRangeAsync(students);
            }
        }

    }
}
