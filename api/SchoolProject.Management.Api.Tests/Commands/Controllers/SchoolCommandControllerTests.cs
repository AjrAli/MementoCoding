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
using SchoolProject.Management.Application.Profiles.Schools;
using SchoolProject.Management.Domain.Entities;
using SchoolProject.Management.Persistence.Context;
using Serilog;
using Serilog.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolProject.Management.Api.Tests.Commands.Controllers
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

        [TestMethod]
        public async Task CheckCreateSchoolReturnSuccess()
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
            if (createSchoolDto == null || _mapper.Map<School>(createSchoolDto) == null)
                throw new Exception("Invalid DTO or null");
            var mockResponseFactory = new Mock<IResponseFactory<CreateSchoolCommandResponse>>();
            _mockSchoolRepo.Setup(x => x.AddAsync(It.IsAny<School>())).Returns(Task.CompletedTask);
            mockResponseFactory.Setup(x => x.CreateResponse()).Returns(new CreateSchoolCommandResponse());
            return new CreateSchoolCommandHandler(_mapper, _mockSchoolRepo.Object, InitUnitOfWork(), mockResponseFactory.Object);
        }
        private UnitOfWork<SchoolManagementDbContext> InitUnitOfWork()
        {
            DbContextOptions<SchoolManagementDbContext> options = new DbContextOptions<SchoolManagementDbContext>();
            var mockDbContext = new Mock<SchoolManagementDbContext>(options);
            mockDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            var mockUnitOfWork = new UnitOfWork<SchoolManagementDbContext>(mockDbContext.Object);
            return mockUnitOfWork;
        }
    }
}
