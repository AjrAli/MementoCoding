using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SchoolProject.Management.Domain.Entities;
using SchoolProject.Management.Persistence.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolProject.Management.Persistence.Tests.Integration_Test.Context
{
    [TestClass]
    public class SchoolManagementDbContextTests
    {
        private readonly DbContextOptions<SchoolManagementDbContext> _inMemoryOptions = new DbContextOptionsBuilder<SchoolManagementDbContext>()
                            .UseInMemoryDatabase(databaseName: "SchoolManagementDb")
                            .Options;
        private readonly List<Student> _students = new List<Student>();
        private readonly List<School> _schools = new List<School>();

        [TestInitialize]
        public void Setup()
        {
            var sqlServerOptions = new DbContextOptionsBuilder<SchoolManagementDbContext>()
                           .UseSqlServer("Server=localhost;Database=SchoolManagementDb;Trusted_Connection=True;MultipleActiveResultSets=True;")
                           .Options;

            using (var sqlServerContext = new SchoolManagementDbContext(sqlServerOptions))
            {
                var schools = sqlServerContext?.Schools?.ToList();
                var students = sqlServerContext?.Students?.ToList();
                if (schools != null && students != null)
                {
                    _schools.AddRange(schools);
                    _students.AddRange(students);
                }
            }
        }

        [TestMethod]
        public async Task Schools_ToListAsync_ReturnListOfSchools()
        {
            //Arrange

            var inMemoryContext = new SchoolManagementDbContext(_inMemoryOptions);
            List<School> listSchools = null;
            // Act
            using (inMemoryContext)
            {
                if (inMemoryContext.Schools.Any())
                {
                    inMemoryContext.Schools.RemoveRange(_schools);
                    inMemoryContext.SaveChanges();
                }
                inMemoryContext.Schools.AddRange(_schools);
                inMemoryContext.SaveChanges();
                listSchools = await inMemoryContext.Schools?.ToListAsync();
            }
            // Assert
            Assert.IsNotNull(listSchools);
        }
        [TestMethod]
        public async Task Students_ToListAsync_ReturnListOfStudents()
        {
            //Arrange

            var inMemoryContext = new SchoolManagementDbContext(_inMemoryOptions);
            List<Student> listStudents = null;
            // Act
            using (inMemoryContext)
            {
                if (inMemoryContext.Students.Any())
                {
                    inMemoryContext.Students.RemoveRange(_students);
                    inMemoryContext.SaveChanges();
                }
                inMemoryContext.Students.AddRange(_students);
                inMemoryContext.SaveChanges();
                listStudents = await inMemoryContext.Students?.ToListAsync();
            }
            // Assert
            Assert.IsNotNull(listStudents);
        }
    }
}
