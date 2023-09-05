using ManagementProject.Domain.Entities;
using ManagementProject.Persistence.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagementProject.Persistence.Seed
{
    public static class DatabaseSeeder
    {
        public async static Task SeedAsync(ManagementProjectDbContext context)
        {
            SeedSchools(context);
            SeedStudents(context);
            await context.SaveChangesAsync();
        }

        private async static void SeedSchools(ManagementProjectDbContext context)
        {
            if (context.Schools != null && !context.Schools.Any())
            {
                var schools = new List<School>
                {
                    new School()
                    {
                        Id = 1,
                        Name = "test",
                        Town = "town",
                        Adress = "adres",
                        Description = "desc"
                    },
                    new School()
                    {
                        Id = 2,
                        Name = "test",
                        Town = "town",
                        Adress = "adres",
                        Description = "desc"
                    }
                };

                await context.Schools.AddRangeAsync(schools);
            }
        }

        private async static void SeedStudents(ManagementProjectDbContext context)
        {
            if (context.Students != null && !context.Students.Any())
            {
                var students = new List<Student>
                {
                    new Student()
                    {
                        Id = 1,
                        FirstName = "Test",
                        LastName = "Test",
                        Adress = "MyAdress",
                        Age = 10,
                        SchoolId = 1
                    },
                    new Student()
                    {
                        Id = 2,
                        FirstName = "Test",
                        LastName = "Test",
                        Adress = "MyAdress",
                        Age = 11,
                        SchoolId = 2
                    }
                };

                await context.Students.AddRangeAsync(students);
            }
        }

    }
}
