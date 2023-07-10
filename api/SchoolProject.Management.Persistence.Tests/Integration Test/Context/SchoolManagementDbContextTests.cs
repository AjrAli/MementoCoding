﻿using Microsoft.EntityFrameworkCore;
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
        public async Task AddSchoolAsync_WhenDatabaseHasSchools_ReturnTrue()
        {
            //Arrange
            School school = new()
            {
                Name = "TestDummy",
                Adress = "TestDummy",
                Description = "TestDummy",
                Town = "TestDummy"
            };
            int result = default;
            School addedSchool = null;
            // Act
            using (var inMemoryContext = new SchoolManagementDbContext(_inMemoryOptions))
            {
                await inMemoryContext.Schools.AddAsync(school);
                result = await inMemoryContext.SaveChangesAsync();
                addedSchool = inMemoryContext.Schools.FirstOrDefault(x => x.Name == school.Name);

            }
            // Assert
            Assert.IsTrue(result == 1);
            Assert.IsTrue(addedSchool != null);
        }
        [TestMethod]
        public async Task AddStudentAsync_WhenDatabaseHasStudents_ReturnTrue()
        {
            //Arrange
            Student student = new()
            {
                FirstName = "TestDummy",
                LastName = "TestDummy",
                Adress = "TestDummy",
                Age = 18
            };
            int result = default;
            Student addedStudent = null;
            // Act
            using (var inMemoryContext = new SchoolManagementDbContext(_inMemoryOptions))
            {
                await inMemoryContext.Students.AddAsync(student);
                result = await inMemoryContext.SaveChangesAsync();
                addedStudent = inMemoryContext.Students.FirstOrDefault(x => x.FirstName == student.FirstName && 
                                                                            x.LastName == student.LastName);

            }
            // Assert
            Assert.IsTrue(result == 1);
            Assert.IsTrue(addedStudent != null);
        }
        [TestMethod]
        public async Task RemoveSchool_WhenDatabaseHasSchools_ReturnTrue()
        {
            //Arrange
            int result = default;
            bool schoolExist = false;
            School schoolToRemove = null;
            // Act
            using (var inMemoryContext = new SchoolManagementDbContext(_inMemoryOptions))
            {
                schoolToRemove = await inMemoryContext.Schools.FirstOrDefaultAsync();
                inMemoryContext.Schools.Remove(schoolToRemove);
                result = await inMemoryContext.SaveChangesAsync();
                schoolExist = inMemoryContext.Schools.Any(x => x.Id == schoolToRemove.Id);

            }
            // Assert
            Assert.IsTrue(result == 1);
            Assert.IsTrue(schoolExist == false);
        }
        [TestMethod]
        public async Task RemoveStudent_WhenDatabaseHasStudents_ReturnTrue()
        {
            //Arrange
            int result = default;
            bool studentExist = false;
            Student studentToRemove = null;
            // Act
            using (var inMemoryContext = new SchoolManagementDbContext(_inMemoryOptions))
            {
                studentToRemove = await inMemoryContext.Students.FirstOrDefaultAsync();
                inMemoryContext.Students.Remove(studentToRemove);
                result = await inMemoryContext.SaveChangesAsync();
                studentExist = inMemoryContext.Students.Any(x => x.Id == studentToRemove.Id);

            }
            // Assert
            Assert.IsTrue(result == 1);
            Assert.IsTrue(studentExist == false);
        }
        [TestMethod]
        public async Task UpdateSchool_WhenDatabaseHasSchools_ReturnTrue()
        {
            //Arrange
            int result = default;
            School schoolToUpdate = null;
            string newSchoolName = "NewNameTestDummy";
            bool schoolExist = false;
            // Act
            using (var inMemoryContext = new SchoolManagementDbContext(_inMemoryOptions))
            {
                schoolToUpdate = await inMemoryContext.Schools.FirstOrDefaultAsync();
                schoolToUpdate.Name = newSchoolName;
                inMemoryContext.Schools.Update(schoolToUpdate);
                result = await inMemoryContext.SaveChangesAsync();
                schoolExist = inMemoryContext.Schools.Any(x => x.Name == newSchoolName);

            }
            // Assert
            Assert.IsTrue(result == 1);
            Assert.IsTrue(schoolExist == true);
        }
        [TestMethod]
        public async Task UpdateStudent_WhenDatabaseHasStudents_ReturnTrue()
        {
            //Arrange
            int result = default;
            Student studentToUpdate = null;
            string newStudentFirstName = "NewFirstNameTestDummy";
            bool studentExist = false;
            // Act
            using (var inMemoryContext = new SchoolManagementDbContext(_inMemoryOptions))
            {
                studentToUpdate = await inMemoryContext.Students.FirstOrDefaultAsync();
                studentToUpdate.FirstName = newStudentFirstName;
                inMemoryContext.Students.Update(studentToUpdate);
                result = await inMemoryContext.SaveChangesAsync();
                studentExist = inMemoryContext.Students.Any(x => x.FirstName == newStudentFirstName);

            }
            // Assert
            Assert.IsTrue(result == 1);
            Assert.IsTrue(studentExist == true);
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
