using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObjectsComparer;
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
        private static readonly DbContextOptions<SchoolManagementDbContext> _inMemoryOptions = new DbContextOptionsBuilder<SchoolManagementDbContext>()
                            .UseInMemoryDatabase(databaseName: "SchoolManagementDb")
                            .Options;
        private static readonly DbContextOptions<SchoolManagementDbContext> _inMemoryOptionsEmptyDb = new DbContextOptionsBuilder<SchoolManagementDbContext>()
                    .UseInMemoryDatabase(databaseName: "EmptyDb")
                    .Options;
        private static TestContext? _testContext;


        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            _testContext = testContext;
            var sqlServerOptions = new DbContextOptionsBuilder<SchoolManagementDbContext>()
                           .UseSqlServer("Server=localhost;Database=SchoolManagementDb;Trusted_Connection=True;MultipleActiveResultSets=True;")
                           .Options;
            using (var inMemoryContext = new SchoolManagementDbContext(_inMemoryOptions))
            using (var sqlServerContext = new SchoolManagementDbContext(sqlServerOptions))
            {
                var schools = sqlServerContext?.Schools?.ToList();
                var students = sqlServerContext?.Students?.ToList();
                if (schools != null && students != null)
                {
                    inMemoryContext?.Schools?.AddRange(schools);
                    inMemoryContext?.Students?.AddRange(students);
                    inMemoryContext?.SaveChanges();
                }
            }
        }

        [TestMethod]
        public async Task GetSchoolsAsync_WhenDatabaseHasSchools_ReturnsListOfSchools()
        {
            //Arrange
            List<School> listSchools = null;
            // Act
            using (var inMemoryContext = new SchoolManagementDbContext(_inMemoryOptions))
            {
                listSchools = await inMemoryContext.Schools?.ToListAsync();
            }
            // Assert
            Assert.IsNotNull(listSchools);
        }
        [TestMethod]
        public async Task GetStudentsAsync_WhenDatabaseHasStudents_ReturnsListOfStudents()
        {
            //Arrange
            List<Student> listStudents = null;
            // Act
            using (var inMemoryContext = new SchoolManagementDbContext(_inMemoryOptions))
            {
                listStudents = await inMemoryContext.Students?.ToListAsync();
            }
            // Assert
            Assert.IsNotNull(listStudents);
        }
        [TestMethod]
        public async Task GetSchoolsAsync_WhenDatabaseIsEmpty_ReturnsEmptyList()
        {
            //Arrange
            List<School> listSchools = null;
            // Act
            using (var inMemoryContext = new SchoolManagementDbContext(_inMemoryOptionsEmptyDb))
            {
                listSchools = await inMemoryContext.Schools?.ToListAsync();
            }
            // Assert
            Assert.IsTrue(listSchools?.Count == 0);
        }
        [TestMethod]
        public async Task GetStudentsAsync_WhenDatabaseIsEmpty_ReturnsEmptyList()
        {
            //Arrange
            List<Student> listStudents = null;
            // Act
            using (var inMemoryContext = new SchoolManagementDbContext(_inMemoryOptionsEmptyDb))
            {
                listStudents = await inMemoryContext.Students?.ToListAsync();
            }
            // Assert
            Assert.IsTrue(listStudents?.Count == 0);
        }
    }
}
