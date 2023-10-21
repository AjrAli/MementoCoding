using AutoMapper;
using ManagementProject.Api.Controllers.Commands;
using ManagementProject.Application.Exceptions;
using ManagementProject.Application.Features.Schools;
using ManagementProject.Application.Features.Schools.Commands.CreateSchool;
using ManagementProject.Application.Features.Schools.Commands.DeleteSchool;
using ManagementProject.Application.Features.Schools.Commands.UpdateSchool;
using ManagementProject.Application.Profiles.Schools;
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
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

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
            _dbContextEmpty  = new ManagementProjectDbContext(inMemoryOptionsEmptyDb);
        }
        [TestInitialize]
        public void  AddDataInDb()
        {
            if (_dbContextFilled.Schools?.Count() != 0) return;
            _dbContextFilled.Schools.AddRange(InitSchoolEntity()); 
            _dbContextFilled.SaveChanges();
        }
        [TestMethod]
        public async Task Create_School_ReturnSuccess()
        {
            //Arrange

            var schoolDto = new SchoolDto()
            {
                Name = "test",
                Town = "town",
                Adress = "adres",
                Description = "desc"

            };
            IMediator mediatorMock = MockMediatorCreateSchoolCommandAsync(schoolDto);
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
            var id = _dbContextFilled.Schools.FirstOrDefault()?.Id ?? 0;
            var schoolDto = new SchoolDto()
            {
                Id = id,
                Name = "test",
                Town = "town",
                Adress = "adres",
                Description = "desc"

            };
            IMediator mediatorMock = MockMediatorUpdateSchoolCommand(schoolDto);
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
            var id = _dbContextFilled.Schools.FirstOrDefault()?.Id ?? 0;
            IMediator mediatorMock = MockMediatorDeleteSchoolCommand(id);
            var schoolControllerTest = new SchoolCommandController(mediatorMock, _logger);

            //Act
            var resultSchoolCall = await schoolControllerTest.DeleteSchool(id);

            //Assert
            var success = (((resultSchoolCall as OkObjectResult)?.Value) as DeleteSchoolCommandResponse)?.Success;
            Assert.IsTrue(success);
        }
        [TestMethod]
        public async Task Create_School_ReturnArgumentNullException()
        {
            //Arrange

            SchoolDto? schoolDto = null;
            IMediator mediatorMock = MockMediatorCreateSchoolCommandAsync(null);
            var schoolControllerTest = new SchoolCommandController(mediatorMock, _logger);

            //Act && Assert

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                await schoolControllerTest.CreateSchool(schoolDto);
            });
        }
        [TestMethod]
        public async Task Update_School_ReturnNotFoundException()
        {
            //Arrange

            SchoolDto? schoolDto = new SchoolDto();
            IMediator mediatorMock = MockMediatorUpdateSchoolCommand(null);
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
            IMediator mediatorMock = MockMediatorDeleteSchoolCommand(0);
            var schoolControllerTest = new SchoolCommandController(mediatorMock, _logger);

            //Act && Assert

            await Assert.ThrowsExceptionAsync<NotFoundException>(async () =>
            {
                await schoolControllerTest.DeleteSchool(0);
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
        private IMediator MockMediatorCreateSchoolCommandAsync(SchoolDto dto)
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
            return new CreateSchoolCommandHandler(_mapper, (result == -1) ? _dbContextEmpty : _dbContextFilled);
        }
        private IMediator MockMediatorUpdateSchoolCommand(SchoolDto dto)
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
            return new UpdateSchoolCommandHandler(_mapper, (result == -1) ? _dbContextEmpty : _dbContextFilled);
        }

        private IMediator MockMediatorDeleteSchoolCommand(long id)
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
            return new DeleteSchoolCommandHandler((result == -1) ? _dbContextEmpty : _dbContextFilled);
        }
        private static List<School> InitSchoolEntity()
        {
            return new List<School>()
            {
                new School()
                {
                    Name = "test",
                    Town = "town",
                    Adress = "adres",
                    Description = "desc"
                },
                new School()
                {
                    Name = "test",
                    Town = "town",
                    Adress = "adres",
                    Description = "desc"
                }
            };
        }
    }
}
