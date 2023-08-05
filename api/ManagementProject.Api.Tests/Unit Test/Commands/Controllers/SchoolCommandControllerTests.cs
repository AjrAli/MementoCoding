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
using ManagementProject.Application.Features.Schools;
using ManagementProject.Application.Features.Schools.Commands.CreateSchool;
using ManagementProject.Application.Features.Schools.Commands.DeleteSchool;
using ManagementProject.Application.Features.Schools.Commands.UpdateSchool;
using ManagementProject.Application.Profiles.Schools;
using ManagementProject.Domain.Entities;
using ManagementProject.Persistence.Context;
using Serilog;
using Serilog.Extensions.Logging;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DotNetCore.Results;
using ManagementProject.Application.Exceptions;

namespace ManagementProject.Api.Tests.Unit_Test.Commands.Controllers
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
            Mock<IMediator> mediatorMock = MockMediatorCreateSchoolCommand(schoolDto);
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
            Mock<IMediator> mediatorMock = MockMediatorUpdateSchoolCommand(schoolDto);
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
            Mock<IMediator> mediatorMock = MockMediatorDeleteSchoolCommand(1);
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
            Mock<IMediator> mediatorMock = MockMediatorCreateSchoolCommand(null);
            var schoolControllerTest = new SchoolCommandController(mediatorMock.Object, _logger);

            //Act && Assert

            await Assert.ThrowsExceptionAsync<BadRequestException>(async () =>
            {
                await schoolControllerTest.CreateSchool(schoolDto);
            });
        }
        [TestMethod]
        public async Task Update_School_ReturnNotFoundException()
        {
            //Arrange

            SchoolDto schoolDto = null;
            Mock<IMediator> mediatorMock = MockMediatorUpdateSchoolCommand(null);
            var schoolControllerTest = new SchoolCommandController(mediatorMock.Object, _logger);

            //Act && Assert

            await Assert.ThrowsExceptionAsync<NotFoundException>(async () =>
            {
                await schoolControllerTest.UpdateSchool(schoolDto);
            });
        }
        [TestMethod]
        public async Task Delete_School_ReturnNotFoundException()
        {
            //Arrange
            Mock<IMediator> mediatorMock = MockMediatorDeleteSchoolCommand(0);
            var schoolControllerTest = new SchoolCommandController(mediatorMock.Object, _logger);

            //Act && Assert

            await Assert.ThrowsExceptionAsync<NotFoundException>(async () =>
            {
                await schoolControllerTest.DeleteSchool(0);
            });
        }
        private Mock<IMediator> MockMediatorCreateSchoolCommand(SchoolDto dto)
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<CreateSchoolCommand>(), default)).Returns(
                async (CreateSchoolCommand q, CancellationToken token) =>
                await InitCreateSchoolCommandHandler(dto).Handle(q, token));
            return mediatorMock;
        }
        private CreateSchoolCommandHandler InitCreateSchoolCommandHandler(SchoolDto? createSchoolDto)
        {
            int result = (createSchoolDto == null) ? -1 : 1;
            if(result == -1)
                _mockSchoolRepo.Setup(x => x.AddAsync(It.IsAny<School>())).ThrowsAsync(new BadRequestException("Failed to add school"));
            else
                _mockSchoolRepo.Setup(x => x.AddAsync(It.IsAny<School>())).Returns(Task.CompletedTask);
            return new CreateSchoolCommandHandler(_mapper, _mockSchoolRepo.Object, InitUnitOfWork(result));
        }
        private Mock<IMediator> MockMediatorUpdateSchoolCommand(SchoolDto dto)
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<UpdateSchoolCommand>(), default)).Returns(
                async (UpdateSchoolCommand q, CancellationToken token) =>
                await InitUpdateSchoolCommandHandler(dto).Handle(q, token));
            return mediatorMock;
        }
        private UpdateSchoolCommandHandler InitUpdateSchoolCommandHandler(SchoolDto? updateSchoolDto)
        {
            int result = (updateSchoolDto == null) ? -1 : 1;
            if (result == -1)
                _mockSchoolRepo.Setup(x => x.UpdateAsync(It.IsAny<School>())).ThrowsAsync(new BadRequestException("Failed to update school"));
            else
                _mockSchoolRepo.Setup(x => x.UpdateAsync(It.IsAny<School>())).Returns(Task.CompletedTask);
            _mockSchoolRepo.Setup(x => x.GetAsync(It.IsAny<long>())).ReturnsAsync(InitSchoolEntity());
            return new UpdateSchoolCommandHandler(_mapper, _mockSchoolRepo.Object, InitUnitOfWork(result));
        }

        private Mock<IMediator> MockMediatorDeleteSchoolCommand(long id)
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<DeleteSchoolCommand>(), default)).Returns(
                async (DeleteSchoolCommand q, CancellationToken token) =>
                await InitDeleteSchoolCommandHandler(id).Handle(q, token));
            return mediatorMock;
        }
        private DeleteSchoolCommandHandler InitDeleteSchoolCommandHandler(long id)
        {
            int result = (id == 0) ? -1 : 1;
            if (result == -1)
                _mockSchoolRepo.Setup(x => x.DeleteAsync(It.IsAny<School>())).ThrowsAsync(new BadRequestException("Failed to delete school"));
            else
                _mockSchoolRepo.Setup(x => x.DeleteAsync(It.IsAny<School>())).Returns(Task.CompletedTask);
            _mockSchoolRepo.Setup(x => x.GetAsync(It.IsAny<long>())).ReturnsAsync(InitSchoolEntity(id));
            _mockStudentRepo.Setup(x => x.Any(It.IsAny<Expression<Func<Student, bool>>>())).Returns(false);
            return new DeleteSchoolCommandHandler(_mockSchoolRepo.Object, _mockStudentRepo.Object, InitUnitOfWork(result));
        }
        private UnitOfWork<ManagementProjectDbContext> InitUnitOfWork(int result)
        {
            DbContextOptions<ManagementProjectDbContext> options = new DbContextOptions<ManagementProjectDbContext>();
            var mockDbContext = new Mock<ManagementProjectDbContext>(options);
            mockDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(result);
            var mockUnitOfWork = new UnitOfWork<ManagementProjectDbContext>(mockDbContext.Object);
            return mockUnitOfWork;
        }
        private static School InitSchoolEntity()
        {
            return new School(3, "test", "adres", "town", "desc");
        }
        private static School InitSchoolEntity(long id)
        {
            if (id == 1)
                return new School(1, "test", "adres", "town", "desc");

            return null;
        }
    }
}
