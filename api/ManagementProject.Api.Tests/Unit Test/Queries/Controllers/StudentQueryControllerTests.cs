using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ManagementProject.Api.Controllers.Queries;
using ManagementProject.Application.Exceptions;
using ManagementProject.Application.Features.Students.Queries.GetStudent;
using ManagementProject.Application.Features.Students.Queries.GetStudents;
using ManagementProject.Application.Profiles.Students;
using ManagementProject.Domain.Entities;
using ManagementProject.Persistence.Context;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace ManagementProject.Api.Tests.Unit_Test.Queries.Controllers
{
    [TestClass]
    public class StudentQueryControllerTests
    {
        private readonly ILogger<StudentQueryController> _logger = Substitute.For<ILogger<StudentQueryController>>();

        private readonly IMapper _mapper =
            new MapperConfiguration(x => x.AddProfile<StudentMappingProfile>()).CreateMapper();

        private static TestContext? _testContext;
        private readonly IMediator _mediatorMock = Substitute.For<IMediator>();
        private static ManagementProjectDbContext _dbContextFilled;
        private static ManagementProjectDbContext _dbContextEmpty;

        [ClassInitialize]
        public static void SetupTests(TestContext testContext)
        {
            _testContext = testContext;
            var inMemoryOptionsDb = new DbContextOptionsBuilder<ManagementProjectDbContext>()
                .UseInMemoryDatabase(databaseName: "FakeDB")
                .Options;
            var inMemoryOptionsEmptyDb = new DbContextOptionsBuilder<ManagementProjectDbContext>()
                .UseInMemoryDatabase(databaseName: "EmptyDB")
                .Options;
            _dbContextFilled = new ManagementProjectDbContext(inMemoryOptionsDb);
            _dbContextEmpty = new ManagementProjectDbContext(inMemoryOptionsEmptyDb);
            if (_dbContextFilled.Students.Count() == 0)
            {
                _dbContextFilled.Students?.AddRange(InitListOfStudentEntity());
                _dbContextFilled.SaveChanges();
            }
        }

        [TestMethod]
        public async Task GetStudent_ReturnNotFoundException()
        {
            //Arrange
            long studentIdRequested = 0;
            SetupGetStudentQueryMediator(studentIdRequested);
            var studentControllerTest = new StudentQueryController(_mediatorMock, _logger);

            // Act && Assert
            await Assert.ThrowsExceptionAsync<NotFoundException>(async () =>
            {
                await studentControllerTest.GetStudent(studentIdRequested);
            });
        }

        [TestMethod]
        public async Task GetStudents_ReturnNotFoundException()
        {
            //Arrange
            SetupGetStudentsQueryMediator(false);
            var studentControllerTest = new StudentQueryController(_mediatorMock, _logger);

            // Act && Assert
            await Assert.ThrowsExceptionAsync<NotFoundException>(async () =>
            {
                await studentControllerTest.GetStudents();
            });
        }

        [TestMethod]
        public async Task GetStudent_ReturnCorrectStudentDto()
        {
            //Arrange
            long studentIdRequested = 3;
            var studentDto = InitGetStudentQueryResponse(studentIdRequested)?.StudentDto;
            SetupGetStudentQueryMediator(studentIdRequested);
            var studentControllerTest = new StudentQueryController(_mediatorMock, _logger);

            //Act
            var resultStudentCall = await studentControllerTest.GetStudent(studentIdRequested);

            //Assert
            var modelDto = (((resultStudentCall as OkObjectResult)?.Value) as GetStudentQueryResponse)?.StudentDto;
            Assert.IsNotNull(modelDto);
            Assert.IsTrue(modelDto.Equals(studentDto));
        }

        [TestMethod]
        public async Task GetStudents_ReturnCorrectListOfStudentDto()
        {
            //Arrange
            SetupGetStudentsQueryMediator(true);
            var studentsDto = InitGetStudentsQueryResponse(true)?.StudentsDto;
            var studentControllerTest = new StudentQueryController(_mediatorMock, _logger);

            //Act
            var resultStudentCall = await studentControllerTest.GetStudents();

            //Assert
            var modelAllDto = (((resultStudentCall as OkObjectResult)?.Value) as GetStudentsQueryResponse)?.StudentsDto;
            Assert.IsNotNull(modelAllDto);
            Assert.IsTrue(modelAllDto.SequenceEqual(studentsDto));
        }

        private void SetupGetStudentsQueryMediator(bool isListExpected)
        {
            _mediatorMock.Send(Arg.Any<GetStudentsQuery>(), default).Returns(x =>
            {
                return InitGetStudentsQueryHandler(isListExpected)
                    .Handle(x.Arg<GetStudentsQuery>(), x.Arg<CancellationToken>());
            });
        }

        private void SetupGetStudentQueryMediator(long? id)
        {
            _mediatorMock.Send(Arg.Any<GetStudentQuery>(), default).Returns(x =>
            {
                return InitGetStudentQueryHandler(id).Handle(x.Arg<GetStudentQuery>(), x.Arg<CancellationToken>());
            });
        }


        private GetStudentQueryHandler InitGetStudentQueryHandler(long? id)
        {
            return new GetStudentQueryHandler(_mapper, (id == 0) ? _dbContextEmpty : _dbContextFilled);
        }

        private GetStudentsQueryHandler InitGetStudentsQueryHandler(bool isListExpected)
        {
            return new GetStudentsQueryHandler(_mapper, (isListExpected) ? _dbContextFilled : _dbContextEmpty);
        }

        private GetStudentsQueryResponse InitGetStudentsQueryResponse(bool isExpected)
        {
            return new GetStudentsQueryResponse()
            {
                StudentsDto = (isExpected)
                    ? new List<GetStudentsDto>()
                    {
                        new GetStudentsDto()
                        {
                            Id = 3,
                            FirstName = "Test",
                            LastName = "Test",
                            Adress = "MyAdress",
                            Age = 10,
                            SchoolId = 5,
                            Parentname = "test"
                        },
                        new GetStudentsDto()
                        {
                            Id = 6,
                            FirstName = "Test6",
                            LastName = "Test6",
                            Adress = "MyAdress6",
                            Age = 16,
                            SchoolId = 10,
                            Parentname = "test"
                        },
                    }
                    : null
            };
        }

        private GetStudentQueryResponse InitGetStudentQueryResponse(long id)
        {
            return new GetStudentQueryResponse()
            {
                StudentDto = (id == 3)
                    ? new GetStudentDto()
                    {
                        Id = 3,
                        FirstName = "Test",
                        LastName = "Test",
                        Adress = "MyAdress",
                        Age = 10,
                        SchoolId = 5,
                        Parentname = "test"
                    }
                    : null
            };
        }


        private static List<Student>? InitListOfStudentEntity()
        {
            return new List<Student>()
            {
                new Student()
                {
                    Id = 3,
                    FirstName = "Test",
                    LastName = "Test",
                    Adress = "MyAdress",
                    Age = 10,
                    SchoolId = 5,
                    School = new School()
                        { Id = 5, Name = "test", Town = "town", Adress = "adres", Description = "desc" }
                },
                new Student()
                {
                    Id = 6,
                    FirstName = "Test6",
                    LastName = "Test6",
                    Adress = "MyAdress6",
                    Age = 16,
                    SchoolId = 10,
                    School = new School()
                        { Id = 10, Name = "test", Town = "town", Adress = "adres", Description = "desc" }
                }
            };
        }
    }
}