using AutoMapper;
using DotNetCore.EntityFrameworkCore;
using ManagementProject.Api.Controllers.Commands;
using ManagementProject.Application.Contracts.Persistence;
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
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

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
        private readonly IStudentRepository _mockStudentRepo = Substitute.For<IStudentRepository>();

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

            var studentDto = new StudentDto()
            {
                Id = 3,
                FirstName = "Test",
                LastName = "Test",
                Adress = "MyAdress",
                Age = 10,
                SchoolId = 5
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
            IMediator mediatorMock =  MockMediatorDeleteStudentCommand(1);
            var studentControllerTest = new StudentCommandController(mediatorMock, _logger);

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
            IMediator mediatorMock = MockMediatorCreateStudentCommandAsync(null);
            var studentControllerTest = new StudentCommandController(mediatorMock, _logger);

            //Act && Assert

            await Assert.ThrowsExceptionAsync<BadRequestException>(async () =>
            {
                await studentControllerTest.CreateStudent(studentDto);
            });
        }
        [TestMethod]
        public async Task Update_Student_ReturnNotFoundException()
        {
            //Arrange

            StudentDto studentDto = null;
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
            IMediator mediatorMock =  MockMediatorDeleteStudentCommand(0);
            var studentControllerTest = new StudentCommandController(mediatorMock, _logger);

            //Act && Assert

            await Assert.ThrowsExceptionAsync<NotFoundException>(async () =>
            {
                await studentControllerTest.DeleteStudent(0);
            });
        }
        private IMediator MockMediatorCreateStudentCommandAsync(StudentDto dto)
        {
            var mediatorMock = Substitute.For<IMediator>();
            mediatorMock.Send(Arg.Any<CreateStudentCommand>(), default).Returns(x =>
            {
                return InitCreateStudentCommandHandler(dto).Handle(x.Arg<CreateStudentCommand>(), x.Arg<CancellationToken>());
            });
               
            return mediatorMock;
        }
        private CreateStudentCommandHandler InitCreateStudentCommandHandler(StudentDto? createStudentDto)
        {
            int result = (createStudentDto == null) ? -1 : 1;
            if (result == -1)
                _mockStudentRepo.AddAsync(Arg.Any<Student>()).Returns(x => { throw new BadRequestException("Failed to add student"); });
            else
                _mockStudentRepo.AddAsync(Arg.Any<Student>()).Returns(Task.CompletedTask);
            return new CreateStudentCommandHandler(_mapper, _mockStudentRepo, InitUnitOfWork(result));
        }
        private IMediator MockMediatorUpdateStudentCommand(StudentDto dto)
        {
            var mediatorMock = Substitute.For<IMediator>();
            mediatorMock.Send(Arg.Any<UpdateStudentCommand>(), default).Returns(x =>
            {
                return InitUpdateStudentCommandHandler(dto).Handle(x.Arg<UpdateStudentCommand>(), x.Arg<CancellationToken>());
            });
             
            return mediatorMock;
        }
        private UpdateStudentCommandHandler InitUpdateStudentCommandHandler(StudentDto? updateStudentDto)
        {
            int result = (updateStudentDto == null) ? -1 : 1;
            if (result == -1)
                _mockStudentRepo.UpdateAsync(Arg.Any<Student>()).Returns(x => { throw new BadRequestException("Failed to update student"); });
            else
                _mockStudentRepo.UpdateAsync(Arg.Any<Student>()).Returns(Task.CompletedTask);
            _mockStudentRepo.GetAsync(Arg.Any<long>()).Returns(InitStudentEntity());
            return new UpdateStudentCommandHandler(_mapper, _mockStudentRepo, InitUnitOfWork(result));
        }

        private IMediator MockMediatorDeleteStudentCommand(long id)
        {
            var mediatorMock = Substitute.For<IMediator>();
            mediatorMock.Send(Arg.Any<DeleteStudentCommand>(), default).Returns(x =>
            {
                return InitDeleteStudentCommandHandler(id).Handle(x.Arg<DeleteStudentCommand>(), x.Arg<CancellationToken>());
            });
              
            return mediatorMock;
        }
        private DeleteStudentCommandHandler InitDeleteStudentCommandHandler(long id)
        {
            int result = (id == 0) ? -1 : 1;
            if (result == -1)
                _mockStudentRepo.DeleteAsync(Arg.Any<Student>()).Returns(x => { throw new BadRequestException("Failed to delete student"); });
            else
                _mockStudentRepo.DeleteAsync(Arg.Any<Student>()).Returns(Task.CompletedTask);
            _mockStudentRepo.GetAsync(Arg.Any<long>()).Returns(InitStudentEntity(id));
            _mockStudentRepo.Any(Arg.Any<Expression<Func<Student, bool>>>()).Returns(false);
            return new DeleteStudentCommandHandler(_mockStudentRepo, InitUnitOfWork(result));
        }
        private static UnitOfWork<ManagementProjectDbContext> InitUnitOfWork(int result)
        {
            DbContextOptions<ManagementProjectDbContext> options = new ();
            var mockDbContext = Substitute.For<ManagementProjectDbContext>(options);
            mockDbContext.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(result);
            var mockUnitOfWork = new UnitOfWork<ManagementProjectDbContext>(mockDbContext);
            return mockUnitOfWork;
        }
        private static Student InitStudentEntity()
        {
            return new Student(3, "Test", "Test", 10, "MyAdress", 5);
        }
        private static Student? InitStudentEntity(long id)
        {
            if (id == 1)
                return new Student(1, "Test", "Test", 10, "MyAdress", 5);

            return null;
        }
    }
}
