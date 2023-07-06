using AutoMapper;
using DotNetCore.EntityFrameworkCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SchoolProject.Management.Api.Controllers.Commands;
using SchoolProject.Management.Application.Contracts.Persistence;
using SchoolProject.Management.Application.Features.Response;
using SchoolProject.Management.Application.Features.Schools;
using SchoolProject.Management.Application.Features.Schools.Commands.CreateSchool;
using SchoolProject.Management.Application.Features.Schools.Commands.DeleteSchool;
using SchoolProject.Management.Application.Features.Schools.Commands.UpdateSchool;
using SchoolProject.Management.Application.Profiles.Schools;
using SchoolProject.Management.Domain.Entities;
using SchoolProject.Management.Persistence.Context;
using Serilog;
using Serilog.Extensions.Logging;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolProject.Management.Api.Tests.Unit_Test.Commands.Controllers
{
    [TestClass]
    public partial class SchoolCommandControllerTests
    {
        private readonly ILogger<SchoolCommandController> _logger = new SerilogLoggerFactory(new LoggerConfiguration()
                                                                                          .WriteTo.Debug()
                                                                                          .CreateLogger())
                                                                  .CreateLogger<SchoolCommandController>();
        private readonly IMapper _mapper = new MapperConfiguration(x => x.AddProfile<SchoolMappingProfile>()).CreateMapper();
        private Mock<ISchoolRepository> _mockSchoolRepo = new Mock<ISchoolRepository>();
        private Mock<IStudentRepository> _mockStudentRepo = new Mock<IStudentRepository>();

        [TestMethod]
        public async Task Create_School_ReturnSuccess()
        {
            //Arrange

            var schoolDto = new SchoolDto()
            {
                Id = 3,
                Name = "test",
                Town = "town",
                Adress = "adres",
                Description = "desc"

            };
            Mock<IMediator> mediatorMock = MockMediatorCreateSchoolCommand();
            var schoolControllerTest = new SchoolCommandController(mediatorMock.Object, _logger);

            //Act
            var resultSchoolCall = await schoolControllerTest.CreateSchool(schoolDto);

            //Assert
            var success = (((resultSchoolCall as OkObjectResult)?.Value) as CreateSchoolCommandResponse)?.Success;
            Assert.IsTrue(success);
        }
        [TestMethod]
        public async Task Update_School_ReturnSuccess()
        {
            //Arrange

            var schoolDto = new SchoolDto()
            {
                Id = 3,
                Name = "test",
                Town = "town",
                Adress = "adres",
                Description = "desc"

            };
            Mock<IMediator> mediatorMock = MockMediatorUpdateSchoolCommand();
            var schoolControllerTest = new SchoolCommandController(mediatorMock.Object, _logger);

            //Act
            var resultSchoolCall = await schoolControllerTest.UpdateSchool(schoolDto);

            //Assert
            var success = (((resultSchoolCall as OkObjectResult)?.Value) as UpdateSchoolCommandResponse)?.Success;
            Assert.IsTrue(success);
        }
        [TestMethod]
        public async Task Delete_School_ReturnSuccess()
        {
            //Arrange
            Mock<IMediator> mediatorMock = MockMediatorDeleteSchoolCommand();
            var schoolControllerTest = new SchoolCommandController(mediatorMock.Object, _logger);

            //Act
            var resultSchoolCall = await schoolControllerTest.DeleteSchool(1);

            //Assert
            var success = (((resultSchoolCall as OkObjectResult)?.Value) as DeleteSchoolCommandResponse)?.Success;
            Assert.IsTrue(success);
        }
        [TestMethod]
        public async Task Create_School_ReturnBadRequest()
        {
            //Arrange

            SchoolDto schoolDto = null;
            Mock<IMediator> mediatorMock = MockMediatorCreateSchoolCommand();
            var schoolControllerTest = new SchoolCommandController(mediatorMock.Object, _logger);

            //Act
            var resultSchoolCall = await schoolControllerTest.CreateSchool(schoolDto);

            //Assert
            Assert.IsTrue(resultSchoolCall is BadRequestObjectResult);
            var success = (((resultSchoolCall as BadRequestObjectResult)?.Value) as ErrorResponse)?.Success;
            Assert.IsTrue(!success);
        }
        [TestMethod]
        public async Task Update_School_ReturnBadRequest()
        {
            //Arrange

            SchoolDto schoolDto = null;
            Mock<IMediator> mediatorMock = MockMediatorUpdateSchoolCommand();
            var schoolControllerTest = new SchoolCommandController(mediatorMock.Object, _logger);

            //Act
            var resultSchoolCall = await schoolControllerTest.UpdateSchool(schoolDto);

            //Assert
            Assert.IsTrue(resultSchoolCall is BadRequestObjectResult);
            var success = (((resultSchoolCall as BadRequestObjectResult)?.Value) as ErrorResponse)?.Success;
            Assert.IsTrue(!success);
        }
        [TestMethod]
        public async Task Delete_School_ReturnBadRequest()
        {
            //Arrange
            Mock<IMediator> mediatorMock = MockMediatorDeleteSchoolCommand();
            var schoolControllerTest = new SchoolCommandController(mediatorMock.Object, _logger);

            //Act
            var resultSchoolCall = await schoolControllerTest.DeleteSchool(0);

            //Assert
            Assert.IsTrue(resultSchoolCall is BadRequestObjectResult);
            var success = (((resultSchoolCall as BadRequestObjectResult)?.Value) as ErrorResponse)?.Success;
            Assert.IsTrue(!success);
        }
        private Mock<IMediator> MockMediatorCreateSchoolCommand()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<CreateSchoolCommand>(), default)).Returns(
                async (CreateSchoolCommand q, CancellationToken token) =>
                await InitCreateSchoolCommandHandler(q.School).Handle(q, token));
            return mediatorMock;
        }
        private CreateSchoolCommandHandler InitCreateSchoolCommandHandler(SchoolDto? createSchoolDto)
        {
            var mockResponseFactory = new Mock<IResponseFactory<CreateSchoolCommandResponse>>();
            _mockSchoolRepo.Setup(x => x.AddAsync(It.IsAny<School>())).Returns(Task.CompletedTask);
            mockResponseFactory.Setup(x => x.CreateResponse()).Returns(new CreateSchoolCommandResponse());
            return new CreateSchoolCommandHandler(_mapper, _mockSchoolRepo.Object, InitUnitOfWork(), mockResponseFactory.Object);
        }
        private Mock<IMediator> MockMediatorUpdateSchoolCommand()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<UpdateSchoolCommand>(), default)).Returns(
                async (UpdateSchoolCommand q, CancellationToken token) =>
                await InitUpdateSchoolCommandHandler(q.School).Handle(q, token));
            return mediatorMock;
        }
        private UpdateSchoolCommandHandler InitUpdateSchoolCommandHandler(SchoolDto? updateSchoolDto)
        {
            var mockResponseFactory = new Mock<IResponseFactory<UpdateSchoolCommandResponse>>();
            _mockSchoolRepo.Setup(x => x.GetAsync(updateSchoolDto.Id)).ReturnsAsync(InitSchoolEntity());
            _mockSchoolRepo.Setup(x => x.UpdateAsync(It.IsAny<School>())).Returns(Task.CompletedTask);
            mockResponseFactory.Setup(x => x.CreateResponse()).Returns(new UpdateSchoolCommandResponse());
            return new UpdateSchoolCommandHandler(_mapper, _mockSchoolRepo.Object, InitUnitOfWork(), mockResponseFactory.Object);
        }

        private Mock<IMediator> MockMediatorDeleteSchoolCommand()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<DeleteSchoolCommand>(), default)).Returns(
                async (DeleteSchoolCommand q, CancellationToken token) =>
                await InitDeleteSchoolCommandHandler(q.SchoolId).Handle(q, token));
            return mediatorMock;
        }
        private DeleteSchoolCommandHandler InitDeleteSchoolCommandHandler(long? id)
        {
            var mockResponseFactory = new Mock<IResponseFactory<DeleteSchoolCommandResponse>>();
            _mockSchoolRepo.Setup(x => x.GetAsync(id)).ReturnsAsync(InitSchoolEntity(id));
            _mockStudentRepo.Setup(x => x.Any(It.IsAny<Expression<Func<Student, bool>>>())).Returns(false);
            _mockSchoolRepo.Setup(x => x.DeleteAsync(It.IsAny<School>())).Returns(Task.CompletedTask);
            mockResponseFactory.Setup(x => x.CreateResponse()).Returns(new DeleteSchoolCommandResponse());
            return new DeleteSchoolCommandHandler(_mockSchoolRepo.Object, _mockStudentRepo.Object, InitUnitOfWork(), mockResponseFactory.Object);
        }
        private UnitOfWork<SchoolManagementDbContext> InitUnitOfWork()
        {
            DbContextOptions<SchoolManagementDbContext> options = new DbContextOptions<SchoolManagementDbContext>();
            var mockDbContext = new Mock<SchoolManagementDbContext>(options);
            mockDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            var mockUnitOfWork = new UnitOfWork<SchoolManagementDbContext>(mockDbContext.Object);
            return mockUnitOfWork;
        }
        private static School InitSchoolEntity()
        {
            return new School(3, "test", "adres", "town", "desc");
        }
        private static School InitSchoolEntity(long? id)
        {
            if (id == 1)
                return new School(1, "test", "adres", "town", "desc");

            return null;
        }
    }
}
