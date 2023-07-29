using AutoMapper;
using DotNetCore.EntityFrameworkCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ManagementProject.Api.Controllers.Commands;
using ManagementProject.Application.Contracts.Persistence;
using ManagementProject.Application.Features.Response;
using ManagementProject.Application.Features.Students;
using ManagementProject.Application.Features.Students.Commands.CreateStudent;
using ManagementProject.Application.Features.Students.Commands.DeleteStudent;
using ManagementProject.Application.Features.Students.Commands.UpdateStudent;
using ManagementProject.Application.Profiles.Students;
using ManagementProject.Domain.Entities;
using ManagementProject.Persistence.Context;
using Serilog;
using Serilog.Extensions.Logging;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using ManagementProject.Application.Exceptions;
using ManagementProject.Application.Features.Schools;

namespace ManagementProject.Api.Tests.Unit_Test.Commands.Controllers
{
    [TestClass]
    public partial class StudentCommandControllerTests
    {

        private readonly ILogger<StudentCommandController> _logger = new SerilogLoggerFactory(new LoggerConfiguration()
                                                                                          .WriteTo.Debug()
                                                                                          .CreateLogger())
                                                                  .CreateLogger<StudentCommandController>();
        private readonly IMapper _mapper = new MapperConfiguration(x => x.AddProfile<StudentMappingProfile>()).CreateMapper();
        private Mock<IStudentRepository> _mockStudentRepo = new Mock<IStudentRepository>();
        [TestMethod]
        public async Task Create_Student_ReturnSuccess()
        {
            //Arrange

            var studentDto = new StudentDto()
            {
                Id = 3,
                FirstName = "Test",
                LastName = "Test",
                Adress = "MyAdress",
                Age = 10,
                SchoolId = 5

            };
            Mock<IMediator> mediatorMock = MockMediatorCreateStudentCommand(studentDto);
            var studentControllerTest = new StudentCommandController(mediatorMock.Object, _logger);

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

            var studentDto = new StudentDto()
            {
                Id = 3,
                FirstName = "Test",
                LastName = "Test",
                Adress = "MyAdress",
                Age = 10,
                SchoolId = 5

            };
            Mock<IMediator> mediatorMock = MockMediatorUpdateStudentCommand(studentDto);
            var studentControllerTest = new StudentCommandController(mediatorMock.Object, _logger);

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
            Mock<IMediator> mediatorMock = MockMediatorDeleteStudentCommand(1);
            var studentControllerTest = new StudentCommandController(mediatorMock.Object, _logger);

            //Act
            var resultStudentCall = await studentControllerTest.DeleteStudent(1);

            //Assert
            var success = (((resultStudentCall as OkObjectResult)?.Value) as DeleteStudentCommandResponse)?.Success;
            Assert.IsTrue(success);
        }
        [TestMethod]
        public async Task Create_Student_ReturnBadRequest()
        {
            //Arrange

            StudentDto studentDto = null;
            Mock<IMediator> mediatorMock = MockMediatorCreateStudentCommand(null);
            var studentControllerTest = new StudentCommandController(mediatorMock.Object, _logger);

            //Act && Assert
            await Assert.ThrowsExceptionAsync<BadRequestException>(async () =>
            {
                await studentControllerTest.CreateStudent(studentDto);
            });
        }
        [TestMethod]
        public async Task Update_Student_ReturnBadRequest()
        {
            //Arrange

            StudentDto studentDto = null;
            Mock<IMediator> mediatorMock = MockMediatorUpdateStudentCommand(null);
            var studentControllerTest = new StudentCommandController(mediatorMock.Object, _logger);

            //Act && Assert
            await Assert.ThrowsExceptionAsync<BadRequestException>(async () =>
            {
                await studentControllerTest.UpdateStudent(studentDto);
            });
        }
        [TestMethod]
        public async Task Delete_Student_ReturnBadRequest()
        {
            //Arrange
            Mock<IMediator> mediatorMock = MockMediatorDeleteStudentCommand(0);
            var studentControllerTest = new StudentCommandController(mediatorMock.Object, _logger);

            //Act && Assert
            await Assert.ThrowsExceptionAsync<BadRequestException>(async () =>
            {
                await studentControllerTest.DeleteStudent(0);
            });
        }
        private Mock<IMediator> MockMediatorCreateStudentCommand(StudentDto dto)
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<CreateStudentCommand>(), default)).Returns(
                async (CreateStudentCommand q, CancellationToken token) =>
                await InitCreateStudentCommandHandler(dto).Handle(q, token));
            return mediatorMock;
        }
        private CreateStudentCommandHandler InitCreateStudentCommandHandler(StudentDto? createStudentDto)
        {
            int result = (createStudentDto == null) ? -1 : 1;
            var mockResponseFactory = new Mock<IResponseFactory<CreateStudentCommandResponse>>();
            _mockStudentRepo.Setup(x => x.AddAsync(It.IsAny<Student>())).Returns(Task.CompletedTask);
            mockResponseFactory.Setup(x => x.CreateResponse()).Returns(new CreateStudentCommandResponse());
            return new CreateStudentCommandHandler(_mapper, _mockStudentRepo.Object, InitUnitOfWork(result), mockResponseFactory.Object);
        }
        private Mock<IMediator> MockMediatorUpdateStudentCommand(StudentDto dto)
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<UpdateStudentCommand>(), default)).Returns(
                async (UpdateStudentCommand q, CancellationToken token) =>
                await InitUpdateStudentCommandHandler(dto).Handle(q, token));
            return mediatorMock;
        }
        private UpdateStudentCommandHandler InitUpdateStudentCommandHandler(StudentDto? updateStudentDto)
        {
            int result = (updateStudentDto == null) ? -1 : 1;
            var mockResponseFactory = new Mock<IResponseFactory<UpdateStudentCommandResponse>>();
            _mockStudentRepo.Setup(x => x.GetAsync(It.IsAny<long>())).ReturnsAsync(InitStudentEntity());
            _mockStudentRepo.Setup(x => x.UpdateAsync(It.IsAny<Student>())).Returns(Task.CompletedTask);
            mockResponseFactory.Setup(x => x.CreateResponse()).Returns(new UpdateStudentCommandResponse());
            return new UpdateStudentCommandHandler(_mapper, _mockStudentRepo.Object, InitUnitOfWork(result), mockResponseFactory.Object);
        }

        private Mock<IMediator> MockMediatorDeleteStudentCommand(long Id)
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<DeleteStudentCommand>(), default)).Returns(
                async (DeleteStudentCommand q, CancellationToken token) =>
                await InitDeleteStudentCommandHandler(q.StudentId).Handle(q, token));
            return mediatorMock;
        }
        private DeleteStudentCommandHandler InitDeleteStudentCommandHandler(long id)
        {
            int result = (id == 0) ? -1 : 1;
            var mockResponseFactory = new Mock<IResponseFactory<DeleteStudentCommandResponse>>();
            _mockStudentRepo.Setup(x => x.GetAsync(It.IsAny<long>())).ReturnsAsync(InitStudentEntity(id));
            _mockStudentRepo.Setup(x => x.Any(It.IsAny<Expression<Func<Student, bool>>>())).Returns(false);
            _mockStudentRepo.Setup(x => x.DeleteAsync(It.IsAny<Student>())).Returns(Task.CompletedTask);
            mockResponseFactory.Setup(x => x.CreateResponse()).Returns(new DeleteStudentCommandResponse());
            return new DeleteStudentCommandHandler(_mockStudentRepo.Object, InitUnitOfWork(result), mockResponseFactory.Object);
        }
        private UnitOfWork<ManagementProjectDbContext> InitUnitOfWork(int result)
        {
            DbContextOptions<ManagementProjectDbContext> options = new DbContextOptions<ManagementProjectDbContext>();
            var mockDbContext = new Mock<ManagementProjectDbContext>(options);
            mockDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(result);
            var mockUnitOfWork = new UnitOfWork<ManagementProjectDbContext>(mockDbContext.Object);
            return mockUnitOfWork;
        }
        private static Student InitStudentEntity()
        {
            return new Student(3, "Test", "Test", 10, "MyAdress", 5);
        }
        private static Student InitStudentEntity(long id)
        {
            if (id == 1) 
                return new Student(1, "Test", "Test", 10, "MyAdress", 5);
            
            return null;
        }
    }
}
