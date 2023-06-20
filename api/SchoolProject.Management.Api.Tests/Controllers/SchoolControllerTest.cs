using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockQueryable.Moq;
using Moq;
using ObjectsComparer;
using SchoolProject.Management.Api.Controllers;
using SchoolProject.Management.Application.Contracts.Persistence;
using SchoolProject.Management.Application.Features.Response;
using SchoolProject.Management.Application.Features.Schools.Queries.GetSchool;
using SchoolProject.Management.Application.Features.Schools.Queries.GetSchools;
using SchoolProject.Management.Application.Profiles.Schools;
using SchoolProject.Management.Domain.Entities;
using Serilog;
using Serilog.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolProject.Management.Api.Tests.Controllers
{
    [TestClass]
    public class SchoolControllerTest
    {
        private readonly ILogger<SchoolController> _logger = new SerilogLoggerFactory(new LoggerConfiguration()
                                                                                          .WriteTo.Debug()
                                                                                          .CreateLogger())
                                                                  .CreateLogger<SchoolController>();
        private readonly IMapper _mapper = new MapperConfiguration(x => x.AddProfile<SchoolMappingProfile>()).CreateMapper();
        private GetSchoolDto? _schoolDto;
        private List<GetSchoolDto>? _allSchoolDto;
        private IBaseResponse? _schoolResponse;
        private static TestContext? _testContext;
        private Mock<ISchoolRepository> _mockSchoolRepo = new Mock<ISchoolRepository>();
        private Mock<IStudentRepository> _mockStudentRepo = new Mock<IStudentRepository>();

        [ClassInitialize]
        public static void SetupTests(TestContext testContext)
        {
            _testContext = testContext;
        }
        [TestMethod]
        public async Task CheckWhenGetSchoolReturnNullSchoolDto()
        {
            //Arrange
            long schoolIdRequested = 0;
            _schoolDto = InitSchoolDto(schoolIdRequested);
            _schoolResponse = InitGetSchoolQueryResponse();
            Mock<IMediator> mediatorMock = MockMediatorGetSchoolQuery();
            var schoolControllerTest = new SchoolController(mediatorMock.Object, _logger);

            //Act
            var resultSchoolCall = await schoolControllerTest.GetSchool(schoolIdRequested);

            //Assert
            Assert.IsNotNull(resultSchoolCall);
            Assert.IsTrue(resultSchoolCall is NotFoundResult);
        }
        [TestMethod]
        public async Task CheckWhenGetSchoolsReturnNullListSchoolDto()
        {
            //Arrange
            _allSchoolDto = InitListOfSchoolDto(false);
            _schoolResponse = InitGetSchoolsQueryResponse();
            Mock<IMediator> mediatorMock = MockMediatorGetSchoolsQuery(false);
            var schoolControllerTest = new SchoolController(mediatorMock.Object, _logger);

            //Act
            var resultSchoolCall = await schoolControllerTest.GetSchools();

            //Assert
            Assert.IsNotNull(resultSchoolCall);
            Assert.IsTrue(resultSchoolCall is NotFoundResult);
        }
        [TestMethod]
        public async Task CheckIfGetSchoolReturnCorrectSchoolDto()
        {
            //Arrange
            long schoolIdRequested = 3;
            _schoolDto = InitSchoolDto(schoolIdRequested);
            _schoolResponse = InitGetSchoolQueryResponse();
            Mock<IMediator> mediatorMock = MockMediatorGetSchoolQuery();
            var schoolControllerTest = new SchoolController(mediatorMock.Object, _logger);

            //Act
            var resultSchoolCall = await schoolControllerTest.GetSchool(schoolIdRequested);

            //Assert
            var modelDto = (((resultSchoolCall as OkObjectResult)?.Value) as GetSchoolQueryResponse)?.SchoolDto;
            Assert.IsNotNull(modelDto);
            bool resultCompare = CompareSchoolDtoReceivedByTheExpected(modelDto);
            Assert.IsTrue(resultCompare);
        }


        [TestMethod]
        public async Task CheckIfGetSchoolsReturnCorrectListOfSchoolDto()
        {
            //Arrange
            _allSchoolDto = InitListOfSchoolDto(true);
            _schoolResponse = InitGetSchoolsQueryResponse();
            Mock<IMediator> mediatorMock = MockMediatorGetSchoolsQuery(true);
            var schoolControllerTest = new SchoolController(mediatorMock.Object, _logger);

            //Act
            var resultSchoolCall = await schoolControllerTest.GetSchools();

            //Assert
            var modelAllDto = (((resultSchoolCall as OkObjectResult)?.Value) as GetSchoolsQueryResponse)?.SchoolsDto;
            Assert.IsNotNull(modelAllDto);
            bool resultCompare = CompareListOfSchoolDtoReceivedByListExpected(modelAllDto);
            Assert.IsTrue(resultCompare);

        }

        private Mock<IMediator> MockMediatorGetSchoolsQuery(bool isListExpected)
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<GetSchoolsQuery>(), default)).Returns(
                async (GetSchoolsQuery q, CancellationToken token) =>
                await InitGetSchoolsQueryHandler(isListExpected).Handle(q, token));
            return mediatorMock;
        }

        private Mock<IMediator> MockMediatorGetSchoolQuery()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<GetSchoolQuery>(), default)).Returns(
                async (GetSchoolQuery q, CancellationToken token) =>
                await InitGetSchoolQueryHandler(q.SchoolId).Handle(q, token));
            return mediatorMock;
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
            var mockResponseFactory = new Mock<IResponseFactory<GetSchoolQueryResponse>>();
            _mockSchoolRepo.Setup(x => x.GetAsync(It.IsAny<long>())).ReturnsAsync(InitSchoolEntity(id));
            mockResponseFactory.Setup(x => x.CreateResponse()).Returns(new GetSchoolQueryResponse());
            return new GetSchoolQueryHandler(_mapper, _mockSchoolRepo.Object, _mockStudentRepo.Object, mockResponseFactory.Object);
        }

        private GetSchoolsQueryHandler InitGetSchoolsQueryHandler(bool isListExpected)
        {

            var mockResponseFactory = new Mock<IResponseFactory<GetSchoolsQueryResponse>>();
            var test = InitQueryOfSchoolEntity();
            _mockSchoolRepo.Setup(x => x.GetDbSetQueryable()).Returns(isListExpected ? InitListOfSchoolEntity().BuildMock() : null);
            mockResponseFactory.Setup(x => x.CreateResponse()).Returns(new GetSchoolsQueryResponse());
            return new GetSchoolsQueryHandler(_mapper, _mockSchoolRepo.Object, _mockStudentRepo.Object, mockResponseFactory.Object);
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
        private IQueryable<School> InitQueryOfSchoolEntity()
        {
            return _mockSchoolRepo.Object.GetDbSetQueryable();
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
