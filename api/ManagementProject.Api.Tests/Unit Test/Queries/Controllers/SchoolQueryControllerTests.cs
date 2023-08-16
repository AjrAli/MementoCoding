using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ManagementProject.Api.Controllers.Queries;
using ManagementProject.Application.Contracts.Persistence;
using ManagementProject.Application.Exceptions;
using ManagementProject.Application.Features.Response;
using ManagementProject.Application.Features.Schools.Queries.GetSchool;
using ManagementProject.Application.Features.Schools.Queries.GetSchools;
using ManagementProject.Application.Profiles.Schools;
using ManagementProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using ObjectsComparer;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MockQueryable;
using MockQueryable.NSubstitute;

namespace ManagementProject.Api.Tests.Unit_Test.Queries.Controllers
{
    [TestClass]
    public class SchoolQueryControllerTests
    {
        private readonly ILogger<SchoolQueryController> _logger = Substitute.For<ILogger<SchoolQueryController>>();
        private readonly IMapper _mapper = new MapperConfiguration(x => x.AddProfile<SchoolMappingProfile>()).CreateMapper();
        private GetSchoolDto? _schoolDto;
        private List<GetSchoolDto>? _allSchoolDto;
        private IBaseResponse? _schoolResponse;
        private static TestContext? _testContext;
        private readonly IMediator _mediatorMock = Substitute.For<IMediator>();
        private readonly ISchoolRepository _mockSchoolRepo = Substitute.For<ISchoolRepository>();
        private readonly IStudentRepository _mockStudentRepo = Substitute.For<IStudentRepository>();

        [ClassInitialize]
        public static void SetupTests(TestContext testContext)
        {
            _testContext = testContext;
        }

        [TestMethod]
        public async Task GetSchool_ReturnNotFoundException()
        {
            //Arrange
            long schoolIdRequested = 0;
            SetupGetSchoolQueryResponse(schoolIdRequested);
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
            SetupGetSchoolsQueryResponse(false);
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
            SetupGetSchoolQueryResponse(schoolIdRequested);
            var schoolControllerTest = new SchoolQueryController(_mediatorMock, _logger);

            //Act
            var resultSchoolCall = await schoolControllerTest.GetSchool(schoolIdRequested);

            //Assert
            var modelDto = (((resultSchoolCall as OkObjectResult)?.Value) as GetSchoolQueryResponse)?.SchoolDto;
            Assert.IsNotNull(modelDto);
            bool resultCompare = CompareSchoolDtoReceivedByTheExpected(modelDto);
            Assert.IsTrue(resultCompare);
        }

        [TestMethod]
        public async Task GetSchools_ReturnCorrectListOfSchoolDto()
        {
            //Arrange
            SetupGetSchoolsQueryResponse(true);
            var schoolControllerTest = new SchoolQueryController(_mediatorMock, _logger);

            //Act
            var resultSchoolCall = await schoolControllerTest.GetSchools();

            //Assert
            var modelAllDto = (((resultSchoolCall as OkObjectResult)?.Value) as GetSchoolsQueryResponse)?.SchoolsDto;
            Assert.IsNotNull(modelAllDto);
            bool resultCompare = CompareListOfSchoolDtoReceivedByListExpected(modelAllDto);
            Assert.IsTrue(resultCompare);
        }

        private void SetupGetSchoolsQueryResponse(bool isListExpected)
        {
            _allSchoolDto = InitListOfSchoolDto(isListExpected);
            _schoolResponse = InitGetSchoolsQueryResponse();
            _mediatorMock.Send(Arg.Any<GetSchoolsQuery>(), default).Returns(x =>
            {
                return InitGetSchoolsQueryHandler(isListExpected).Handle(x.Arg<GetSchoolsQuery>(), x.Arg<CancellationToken>());
            });
        }

        private void SetupGetSchoolQueryResponse(long? id)
        {
            _schoolDto = InitSchoolDto(id);
            _schoolResponse = InitGetSchoolQueryResponse();
            _mediatorMock.Send(Arg.Any<GetSchoolQuery>(), default).Returns(x =>
            {
                return InitGetSchoolQueryHandler(id).Handle(x.Arg<GetSchoolQuery>(), x.Arg<CancellationToken>());
            });
               
        }

        private bool CompareSchoolDtoReceivedByTheExpected(GetSchoolDto modelDto)
        {
            var comparerDto = new ObjectsComparer.Comparer<GetSchoolDto>();
            var schoolResponse = _schoolResponse as GetSchoolQueryResponse;
            bool resultCompare = comparerDto.Compare(schoolResponse?.SchoolDto!, modelDto, out var differences);
            WriteOnConsoleDifferencesIfNotEqual(differences);
            return resultCompare;
        }

        private bool CompareListOfSchoolDtoReceivedByListExpected(List<GetSchoolsDto> modelAllDto)
        {
            var comparerDto = new ObjectsComparer.Comparer<List<GetSchoolsDto>>();
            var schoolsResponse = _schoolResponse as GetSchoolsQueryResponse;
            bool resultCompare = comparerDto.Compare(schoolsResponse?.SchoolsDto!, modelAllDto, out var differences);
            WriteOnConsoleDifferencesIfNotEqual(differences);
            return resultCompare;
        }

        private static void WriteOnConsoleDifferencesIfNotEqual(IEnumerable<Difference> differences)
        {
            foreach (var difference in differences)
            {
                _testContext?.WriteLine($"Value 1 : {difference.Value1} and  Value 2 : {difference.Value2} aren't equal!");
            }
        }

        private GetSchoolQueryHandler InitGetSchoolQueryHandler(long? id)
        {
            _mockSchoolRepo.GetAsync(Arg.Any<long>()).Returns(InitSchoolEntity(id));
            return new GetSchoolQueryHandler(_mapper, _mockSchoolRepo);
        }

        private GetSchoolsQueryHandler InitGetSchoolsQueryHandler(bool isListExpected)
        {

            _mockSchoolRepo.GetDbSetQueryable().Returns(isListExpected ? InitListOfSchoolEntity().BuildMock() : null);
            _mockSchoolRepo.CountAsync().Returns(isListExpected ? InitListOfSchoolEntity().Count : 0);
            return new GetSchoolsQueryHandler(_mockSchoolRepo, _mockStudentRepo);
        }
        private GetSchoolsQueryResponse InitGetSchoolsQueryResponse()
        {
            return new GetSchoolsQueryResponse()
            {
                SchoolsDto = _mapper.Map<List<GetSchoolsDto>>(_allSchoolDto)
            };
        }
        private GetSchoolQueryResponse InitGetSchoolQueryResponse()
        {
            return new GetSchoolQueryResponse()
            {
                SchoolDto = _mapper.Map<GetSchoolDto>(_schoolDto)
            };
        }
        private static List<GetSchoolDto>? InitListOfSchoolDto(bool isExpected)
        {
            if (isExpected)
                return new List<GetSchoolDto>()
                {
                    new GetSchoolDto()
                    {
                        Id = 3,
                        Name = "test",
                        Town = "town",
                        Adress = "adres",
                        Description = "desc"
                    },
                    new GetSchoolDto()
                    {
                        Id = 6,
                        Name = "test",
                        Town = "town",
                        Adress = "adres",
                        Description = "desc"
                    },
                };
            return null;
        }

        private static List<School> InitListOfSchoolEntity()
        {
            return new List<School>()
                {
                     new School(3, "test", "adres", "town", "desc"),
                     new School(6, "test", "adres", "town", "desc")
                };
        }
        private static GetSchoolDto? InitSchoolDto(long? id)
        {
            if (id == 3)
                return new GetSchoolDto()
                {
                    Id = 3,
                    Name = "test",
                    Town = "town",
                    Adress = "adres",
                    Description = "desc"

                };
            return null;
        }
        private static School? InitSchoolEntity(long? id)
        {
            if (id == 3)
                return new School(3, "test", "adres", "town", "desc");
            return null;
        }
    }
}
