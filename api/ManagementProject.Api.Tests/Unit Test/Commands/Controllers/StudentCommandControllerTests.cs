using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ManagementProject.Api.Controllers.Commands;
using ManagementProject.Application.Exceptions;
using ManagementProject.Application.Features.Students;
using ManagementProject.Application.Features.Students.Commands.CreateStudent;
using ManagementProject.Application.Features.Students.Commands.DeleteStudent;
using ManagementProject.Application.Features.Students.Commands.UpdateStudent;
using ManagementProject.Application.Profiles.Students;
using ManagementProject.Domain.Entities;
using ManagementProject.Persistence.Context;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Serilog;
using Serilog.Extensions.Logging;

namespace ManagementProject.Api.Tests.Unit_Test.Commands.Controllers
{
    [TestClass]
    public partial class StudentCommandControllerTests
    {
        private readonly ILogger<StudentCommandController> _logger = new SerilogLoggerFactory(new LoggerConfiguration()
                .WriteTo.Debug()
                .CreateLogger())
            .CreateLogger<StudentCommandController>();

        private readonly IMapper _mapper =
            new MapperConfiguration(x => x.AddProfile<StudentMappingProfile>()).CreateMapper();

        private static ManagementProjectDbContext _dbContextFilled;
        private static ManagementProjectDbContext _dbContextEmpty;
        private static TestContext? _testContext;

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
        }
        [TestInitialize]
        public void  AddDataInDb()
        {
            if (_dbContextFilled.Students?.Count() != 0) return;
            _dbContextFilled.Students.AddRange(InitStudentEntity()); 
            _dbContextFilled.SaveChanges();
        }
        [TestMethod]
        public async Task Create_Student_ReturnSuccess()
        {
            //Arrange

            var studentDto = new StudentDto()
            {
                FirstName = "Test",
                LastName = "Test",
                Adress = "MyAdress",
                Age = 10,
                SchoolId = 20
            };
            IMediator mediatorMock = MockMediatorCreateStudentCommandAsync(studentDto);
            var studentControllerTest = new StudentCommandController(mediatorMock, _logger);

            //Act
            var resultStudentCall = await studentControllerTest.CreateStudent(studentDto);

            //Assert
            var success = (((resultStudentCall as OkObjectResult)?.Value) as CreateStudentCommandResponse)?.Success;
            Assert.IsTrue(success);
        }

        [TestMethod]
        public async Task Update_Student_ReturnSuccess()
        {
            //Arrange
            var id = _dbContextFilled.Students.FirstOrDefault()?.Id ?? 0;
            var studentDto = new StudentDto()
            {
                Id = id,
                FirstName = "Test",
                LastName = "Test",
                Adress = "MyAdress",
                Age = 10,
                SchoolId = 20
            };
            IMediator mediatorMock = MockMediatorUpdateStudentCommand(studentDto);
            var studentControllerTest = new StudentCommandController(mediatorMock, _logger);

            //Act
            var resultStudentCall = await studentControllerTest.UpdateStudent(studentDto);

            //Assert
            var success = (((resultStudentCall as OkObjectResult)?.Value) as UpdateStudentCommandResponse)?.Success;
            Assert.IsTrue(success);
        }

        [TestMethod]
        public async Task Delete_Student_ReturnSuccess()
        {
            //Arrange
            var id = _dbContextFilled.Students.FirstOrDefault()?.Id ?? 0;
            IMediator mediatorMock = MockMediatorDeleteStudentCommand(id);
            var studentControllerTest = new StudentCommandController(mediatorMock, _logger);

            //Act
            var resultStudentCall = await studentControllerTest.DeleteStudent(id);

            //Assert
            var success = (((resultStudentCall as OkObjectResult)?.Value) as DeleteStudentCommandResponse)?.Success;
            Assert.IsTrue(success);
        }

        [TestMethod]
        public async Task Create_Student_ReturnArgumentNullException()
        {
            //Arrange

            StudentDto studentDto = null;
            IMediator mediatorMock = MockMediatorCreateStudentCommandAsync(null);
            var studentControllerTest = new StudentCommandController(mediatorMock, _logger);

            //Act && Assert

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                await studentControllerTest.CreateStudent(studentDto);
            });
        }

        [TestMethod]
        public async Task Update_Student_ReturnNotFoundException()
        {
            //Arrange

            StudentDto studentDto = new StudentDto();
            IMediator mediatorMock = MockMediatorUpdateStudentCommand(null);
            var studentControllerTest = new StudentCommandController(mediatorMock, _logger);

            //Act && Assert

            await Assert.ThrowsExceptionAsync<NotFoundException>(async () =>
            {
                await studentControllerTest.UpdateStudent(studentDto);
            });
        }

        [TestMethod]
        public async Task Delete_Student_ReturnNotFoundException()
        {
            //Arrange
            IMediator mediatorMock = MockMediatorDeleteStudentCommand(0);
            var studentControllerTest = new StudentCommandController(mediatorMock, _logger);

            //Act && Assert

            await Assert.ThrowsExceptionAsync<NotFoundException>(async () =>
            {
                await studentControllerTest.DeleteStudent(0);
            });
        }
        [TestCleanup]
        public void  ResetDb()
        {
            if (_dbContextFilled?.Students?.Count() > 0)
            {
                _dbContextFilled.Students.RemoveRange(_dbContextFilled.Students); 
                _dbContextFilled.SaveChanges();
            }
            if (_dbContextFilled?.Schools?.Count() > 0)
            {
                _dbContextFilled.Schools.RemoveRange(_dbContextFilled.Schools); 
                _dbContextFilled.SaveChanges();
            }
        }
        private IMediator MockMediatorCreateStudentCommandAsync(StudentDto dto)
        {
            var mediatorMock = Substitute.For<IMediator>();
            mediatorMock.Send(Arg.Any<CreateStudentCommand>(), default).Returns(x =>
            {
                return InitCreateStudentCommandHandler(dto)
                    .Handle(x.Arg<CreateStudentCommand>(), x.Arg<CancellationToken>());
            });

            return mediatorMock;
        }

        private CreateStudentCommandHandler InitCreateStudentCommandHandler(StudentDto? createStudentDto)
        {
            int result = (createStudentDto == null) ? -1 : 1;
            return new CreateStudentCommandHandler(_mapper, (result == -1) ? _dbContextEmpty : _dbContextFilled);
        }

        private IMediator MockMediatorUpdateStudentCommand(StudentDto dto)
        {
            var mediatorMock = Substitute.For<IMediator>();
            mediatorMock.Send(Arg.Any<UpdateStudentCommand>(), default).Returns(x =>
            {
                return InitUpdateStudentCommandHandler(dto)
                    .Handle(x.Arg<UpdateStudentCommand>(), x.Arg<CancellationToken>());
            });

            return mediatorMock;
        }

        private UpdateStudentCommandHandler InitUpdateStudentCommandHandler(StudentDto? updateStudentDto)
        {
            int result = (updateStudentDto == null) ? -1 : 1;
            return new UpdateStudentCommandHandler(_mapper, (result == -1) ? _dbContextEmpty : _dbContextFilled);
        }

        private IMediator MockMediatorDeleteStudentCommand(long id)
        {
            var mediatorMock = Substitute.For<IMediator>();
            mediatorMock.Send(Arg.Any<DeleteStudentCommand>(), default).Returns(x =>
            {
                return InitDeleteStudentCommandHandler(id)
                    .Handle(x.Arg<DeleteStudentCommand>(), x.Arg<CancellationToken>());
            });

            return mediatorMock;
        }

        private DeleteStudentCommandHandler InitDeleteStudentCommandHandler(long id)
        {
            int result = (id == 0) ? -1 : 1;
            return new DeleteStudentCommandHandler((result == -1) ? _dbContextEmpty : _dbContextFilled);
        }

        private static List<Student> InitStudentEntity()
        {
            return new List<Student>()
            {
                new Student()
                {
                    FirstName = "Test",
                    LastName = "Test",
                    Adress = "MyAdress",
                    Age = 10,
                    SchoolId = 5,
                    School = new School()
                        { Id = 20, Name = "test", Town = "town", Adress = "adres", Description = "desc" }
                },
                new Student()
                {
                    FirstName = "Test",
                    LastName = "Test",
                    Adress = "MyAdress",
                    Age = 10,
                    SchoolId = 10,
                    School = new School()
                        { Id = 30, Name = "test", Town = "town", Adress = "adres", Description = "desc" }
                }
            };
        }
    }
}