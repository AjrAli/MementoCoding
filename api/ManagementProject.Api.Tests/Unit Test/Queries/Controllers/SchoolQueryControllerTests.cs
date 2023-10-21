using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ManagementProject.Api.Controllers.Queries;
using ManagementProject.Application.Exceptions;
using ManagementProject.Application.Features.Schools.Queries.GetSchool;
using ManagementProject.Application.Features.Schools.Queries.GetSchools;
using ManagementProject.Application.Profiles.Schools;
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
    public class SchoolQueryControllerTests
    {
        private readonly ILogger<SchoolQueryController> _logger = Substitute.For<ILogger<SchoolQueryController>>();

        private readonly IMapper _mapper =
            new MapperConfiguration(x => x.AddProfile<SchoolMappingProfile>()).CreateMapper();

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
            if (_dbContextFilled.Schools.Count() == 0)
            {
                _dbContextFilled.Schools?.AddRange(InitListOfSchoolEntity());
                _dbContextFilled.SaveChanges();
            }
        }

        [TestMethod]
        public async Task GetSchool_ReturnNotFoundException()
        {
            //Arrange
            long schoolIdRequested = 0;
            SetupGetSchoolQueryMediator(schoolIdRequested);
            var schoolControllerTest = new SchoolQueryController(_mediatorMock, _logger);

            // Act && Assert
            await Assert.ThrowsExceptionAsync<NotFoundException>(async () =>
            {
                await schoolControllerTest.GetSchool(schoolIdRequested);
            });
        }

        [TestMethod]
        public async Task GetSchools_ReturnNotFoundException()
        {
            //Arrange
            SetupGetSchoolsQueryMediator(false);
            var schoolControllerTest = new SchoolQueryController(_mediatorMock, _logger);

            // Act && Assert
            await Assert.ThrowsExceptionAsync<NotFoundException>(async () =>
            {
                await schoolControllerTest.GetSchools();
            });
        }

        [TestMethod]
        public async Task GetSchool_ReturnCorrectSchoolDto()
        {
            //Arrange
            long schoolIdRequested = 3;
            var schoolDto = InitGetSchoolQueryResponse(schoolIdRequested)?.SchoolDto;
            SetupGetSchoolQueryMediator(schoolIdRequested);
            var schoolControllerTest = new SchoolQueryController(_mediatorMock, _logger);

            //Act
            var resultSchoolCall = await schoolControllerTest.GetSchool(schoolIdRequested);

            //Assert
            var modelDto = (((resultSchoolCall as OkObjectResult)?.Value) as GetSchoolQueryResponse)?.SchoolDto;
            Assert.IsNotNull(modelDto);
            Assert.IsTrue(modelDto.Equals(schoolDto));
        }

        [TestMethod]
        public async Task GetSchools_ReturnCorrectListOfSchoolDto()
        {
            //Arrange
            SetupGetSchoolsQueryMediator(true);
            var schoolsDto = InitGetSchoolsQueryResponse(true)?.SchoolsDto;
            var schoolControllerTest = new SchoolQueryController(_mediatorMock, _logger);

            //Act
            var resultSchoolCall = await schoolControllerTest.GetSchools();

            //Assert
            var modelAllDto = (((resultSchoolCall as OkObjectResult)?.Value) as GetSchoolsQueryResponse)?.SchoolsDto;
            Assert.IsNotNull(modelAllDto);
            Assert.IsTrue(modelAllDto.SequenceEqual(schoolsDto));
        }

        private void SetupGetSchoolsQueryMediator(bool isListExpected)
        {
            _mediatorMock.Send(Arg.Any<GetSchoolsQuery>(), default).Returns(x =>
            {
                return InitGetSchoolsQueryHandler(isListExpected)
                    .Handle(x.Arg<GetSchoolsQuery>(), x.Arg<CancellationToken>());
            });
        }

        private void SetupGetSchoolQueryMediator(long? id)
        {
            _mediatorMock.Send(Arg.Any<GetSchoolQuery>(), default).Returns(x =>
            {
                return InitGetSchoolQueryHandler(id).Handle(x.Arg<GetSchoolQuery>(), x.Arg<CancellationToken>());
            });
        }


        private GetSchoolQueryHandler InitGetSchoolQueryHandler(long? id)
        {
            return new GetSchoolQueryHandler(_mapper, (id == 0) ? _dbContextEmpty : _dbContextFilled);
        }

        private GetSchoolsQueryHandler InitGetSchoolsQueryHandler(bool isListExpected)
        {
            return new GetSchoolsQueryHandler((isListExpected) ? _dbContextFilled : _dbContextEmpty);
        }

        private GetSchoolsQueryResponse InitGetSchoolsQueryResponse(bool isExpected)
        {
            return new GetSchoolsQueryResponse()
            {
                SchoolsDto = (isExpected)
                    ? new List<GetSchoolsDto>()
                    {
                        new GetSchoolsDto()
                        {
                            Id = 3,
                            Name = "test",
                            Town = "town",
                            Adress = "adres",
                            Description = "desc"
                        },
                        new GetSchoolsDto()
                        {
                            Id = 6,
                            Name = "test",
                            Town = "town",
                            Adress = "adres",
                            Description = "desc"
                        },
                    }
                    : null
            };
        }

        private GetSchoolQueryResponse InitGetSchoolQueryResponse(long id)
        {
            return new GetSchoolQueryResponse()
            {
                SchoolDto = (id == 3)
                    ? new GetSchoolDto()
                    {
                        Id = 3,
                        Name = "test",
                        Town = "town",
                        Adress = "adres",
                        Description = "desc"
                    }
                    : null
            };
        }


        private static List<School>? InitListOfSchoolEntity()
        {
            return new List<School>()
            {
                new School()
                {
                    Id = 3,
                    Name = "test",
                    Town = "town",
                    Adress = "adres",
                    Description = "desc"
                },
                new School()
                {
                    Id = 6,
                    Name = "test",
                    Town = "town",
                    Adress = "adres",
                    Description = "desc"
                }
            };
        }
    }
}