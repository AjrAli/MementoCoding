using AutoMapper;
using DotNetCore.EntityFrameworkCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
using NSubstitute;
using Humanizer;
using ManagementProject.Application.Models.Account.Query.Authenticate;

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
        private ISchoolRepository _mockSchoolRepo =  Substitute.For<ISchoolRepository>();
        private IStudentRepository _mockStudentRepo = Substitute.For<IStudentRepository>();

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
            IMediator mediatorMock = await MockMediatorCreateSchoolCommandAsync(schoolDto);
            var schoolControllerTest = new SchoolCommandController(mediatorMock, _logger);

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
            IMediator mediatorMock = await MockMediatorUpdateSchoolCommand(schoolDto);
            var schoolControllerTest = new SchoolCommandController(mediatorMock, _logger);

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
            IMediator mediatorMock = await MockMediatorDeleteSchoolCommand(1);
            var schoolControllerTest = new SchoolCommandController(mediatorMock, _logger);

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
            IMediator mediatorMock = await MockMediatorCreateSchoolCommandAsync(null);
            var schoolControllerTest = new SchoolCommandController(mediatorMock, _logger);

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
            IMediator mediatorMock = await MockMediatorUpdateSchoolCommand(null);
            var schoolControllerTest = new SchoolCommandController(mediatorMock, _logger);

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
            IMediator mediatorMock = await MockMediatorDeleteSchoolCommand(0);
            var schoolControllerTest = new SchoolCommandController(mediatorMock, _logger);

            //Act && Assert

            await Assert.ThrowsExceptionAsync<NotFoundException>(async () =>
            {
                await schoolControllerTest.DeleteSchool(0);
            });
        }
        private async Task<IMediator> MockMediatorCreateSchoolCommandAsync(SchoolDto dto)
        {
            var mediatorMock = Substitute.For<IMediator>();
            mediatorMock.Send(Arg.Any<CreateSchoolCommand>(), default).Returns(x =>
            {
                return InitCreateSchoolCommandHandler(dto).Handle(x.Arg<CreateSchoolCommand>(), x.Arg<CancellationToken>());
            });                   
            return mediatorMock;
        }
        private CreateSchoolCommandHandler InitCreateSchoolCommandHandler(SchoolDto? createSchoolDto)
        {
            int result = (createSchoolDto == null) ? -1 : 1;
            if (result == -1)
                _mockSchoolRepo.AddAsync(Arg.Any<School>()).Returns(x => { throw new BadRequestException("Failed to add school"); });
            else
                _mockSchoolRepo.AddAsync(Arg.Any<School>()).Returns(Task.CompletedTask);
            return new CreateSchoolCommandHandler(_mapper, _mockSchoolRepo, InitUnitOfWork(result));
        }
        private async Task<IMediator> MockMediatorUpdateSchoolCommand(SchoolDto dto)
        {
            var mediatorMock = Substitute.For<IMediator>();
            mediatorMock.Send(Arg.Any<UpdateSchoolCommand>(), default).Returns(x =>
            {
                return InitUpdateSchoolCommandHandler(dto).Handle(x.Arg<UpdateSchoolCommand>(), x.Arg<CancellationToken>());
            });
              
            return mediatorMock;
        }
        private UpdateSchoolCommandHandler InitUpdateSchoolCommandHandler(SchoolDto? updateSchoolDto)
        {
            int result = (updateSchoolDto == null) ? -1 : 1;
            if (result == -1)
                _mockSchoolRepo.UpdateAsync(Arg.Any<School>()).Returns(x => { throw new BadRequestException("Failed to update school"); });
            else
                _mockSchoolRepo.UpdateAsync(Arg.Any<School>()).Returns(Task.CompletedTask);
            _mockSchoolRepo.GetAsync(Arg.Any<long>()).Returns(InitSchoolEntity());
            return new UpdateSchoolCommandHandler(_mapper, _mockSchoolRepo, InitUnitOfWork(result));
        }

        private async Task<IMediator> MockMediatorDeleteSchoolCommand(long id)
        {
            var mediatorMock = Substitute.For<IMediator>();
            mediatorMock.Send(Arg.Any<DeleteSchoolCommand>(), default).Returns(x =>
            {
                return InitDeleteSchoolCommandHandler(id).Handle(x.Arg<DeleteSchoolCommand>(), x.Arg<CancellationToken>());
            });          
            return mediatorMock;
        }
        private DeleteSchoolCommandHandler InitDeleteSchoolCommandHandler(long id)
        {
            int result = (id == 0) ? -1 : 1;
            if (result == -1)
                _mockSchoolRepo.DeleteAsync(Arg.Any<School>()).Returns(x => { throw new BadRequestException("Failed to delete school"); });
            else
                _mockSchoolRepo.DeleteAsync(Arg.Any<School>()).Returns(Task.CompletedTask);
            _mockSchoolRepo.GetAsync(Arg.Any<long>()).Returns(InitSchoolEntity(id));
            _mockStudentRepo.Any(Arg.Any<Expression<Func<Student, bool>>>()).Returns(false);
            return new DeleteSchoolCommandHandler(_mockSchoolRepo, _mockStudentRepo, InitUnitOfWork(result));
        }
        private UnitOfWork<ManagementProjectDbContext> InitUnitOfWork(int result)
        {
            DbContextOptions<ManagementProjectDbContext> options = new DbContextOptions<ManagementProjectDbContext>();
            var mockDbContext = Substitute.For<ManagementProjectDbContext>(options);
            mockDbContext.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(result);
            var mockUnitOfWork = new UnitOfWork<ManagementProjectDbContext>(mockDbContext);
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
