using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockQueryable.Moq;
using Moq;
using ObjectsComparer;
using SchoolProject.Management.Api.Controllers.Queries;
using SchoolProject.Management.Application.Contracts.Persistence;
using SchoolProject.Management.Application.Features.Response;
using SchoolProject.Management.Application.Features.Students.Queries.GetStudent;
using SchoolProject.Management.Application.Features.Students.Queries.GetStudents;
using SchoolProject.Management.Application.Profiles.Students;
using SchoolProject.Management.Domain.Entities;
using Serilog;
using Serilog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolProject.Management.Api.Tests.Unit_Test.Queries.Controllers
{
    [TestClass]
    public class StudentQueryControllerTests
    {

        private readonly ILogger<StudentQueryController> _logger = new SerilogLoggerFactory(new LoggerConfiguration()
                                                                                          .WriteTo.Debug()
                                                                                          .CreateLogger())
                                                                  .CreateLogger<StudentQueryController>();
        private readonly IMapper _mapper = new MapperConfiguration(x => x.AddProfile<StudentMappingProfile>()).CreateMapper();
        private GetStudentDto? _studentDto;
        private List<GetStudentDto>? _allStudentDto;
        private IBaseResponse? _studentResponse;
        private static TestContext? _testContext;
        private Mock<IStudentRepository> _mockStudentRepo = new Mock<IStudentRepository>();

        [ClassInitialize]
        public static void SetupTests(TestContext testContext)
        {
            _testContext = testContext;
        }
        [TestMethod]
        public async Task GetStudent_ReturnNullStudentDto()
        {
            //Arrange
            long studentIdRequested = 0;
            _studentDto = InitStudentDto(studentIdRequested);
            _studentResponse = InitGetStudentQueryResponse();
            Mock<IMediator> mediatorMock = MockMediatorGetStudentQuery();
            var studentControllerTest = new StudentQueryController(mediatorMock.Object, _logger);

            //Act
            var resultStudentCall = await studentControllerTest.GetStudent(studentIdRequested);

            //Assert
            Assert.IsNotNull(resultStudentCall);
            Assert.IsTrue(resultStudentCall is NotFoundResult);
        }
        [TestMethod]
        public async Task GetStudents_ReturnNullListStudentDto()
        {
            //Arrange
            _allStudentDto = InitListOfStudentDto(false);
            _studentResponse = InitGetStudentsQueryResponse();
            Mock<IMediator> mediatorMock = MockMediatorGetStudentsQuery(false);
            var studentControllerTest = new StudentQueryController(mediatorMock.Object, _logger);

            //Act
            var resultStudentCall = await studentControllerTest.GetStudents();

            //Assert
            Assert.IsNotNull(resultStudentCall);
            Assert.IsTrue(resultStudentCall is NotFoundResult);
        }
        [TestMethod]
        public async Task GetStudent_ReturnCorrectStudentDto()
        {
            //Arrange
            long studentIdRequested = 3;
            _studentDto = InitStudentDto(studentIdRequested);
            _studentResponse = InitGetStudentQueryResponse();
            Mock<IMediator> mediatorMock = MockMediatorGetStudentQuery();
            var studentControllerTest = new StudentQueryController(mediatorMock.Object, _logger);

            //Act
            var resultStudentCall = await studentControllerTest.GetStudent(studentIdRequested);

            //Assert
            var modelDto = (((resultStudentCall as OkObjectResult)?.Value) as GetStudentQueryResponse)?.StudentDto;
            Assert.IsNotNull(modelDto);
            bool resultCompare = CompareStudentDtoReceivedByTheExpected(modelDto);
            Assert.IsTrue(resultCompare);
        }


        [TestMethod]
        public async Task GetStudents_ReturnCorrectListOfStudentDto()
        {
            //Arrange
            _allStudentDto = InitListOfStudentDto(true);
            _studentResponse = InitGetStudentsQueryResponse();
            Mock<IMediator> mediatorMock = MockMediatorGetStudentsQuery(true);
            var studentControllerTest = new StudentQueryController(mediatorMock.Object, _logger);

            //Act
            var resultStudentCall = await studentControllerTest.GetStudents();

            //Assert
            var modelAllDto = (((resultStudentCall as OkObjectResult)?.Value) as GetStudentsQueryResponse)?.StudentsDto;
            Assert.IsNotNull(modelAllDto);
            bool resultCompare = CompareListOfStudentDtoReceivedByListExpected(modelAllDto);
            Assert.IsTrue(resultCompare);

        }

        private Mock<IMediator> MockMediatorGetStudentsQuery(bool isListExpected)
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<GetStudentsQuery>(), default)).Returns(
                async (GetStudentsQuery q, CancellationToken token) =>
                await InitGetStudentsQueryHandler(isListExpected).Handle(q, token));
            return mediatorMock;
        }

        private Mock<IMediator> MockMediatorGetStudentQuery()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<GetStudentQuery>(), default)).Returns(
                async (GetStudentQuery q, CancellationToken token) =>
                await InitGetStudentQueryHandler(q.StudentId).Handle(q, token));
            return mediatorMock;
        }

        private bool CompareStudentDtoReceivedByTheExpected(GetStudentDto modelDto)
        {
            var comparerDto = new ObjectsComparer.Comparer<GetStudentDto>();
            var studentResponse = _studentResponse as GetStudentQueryResponse;
            bool resultCompare = comparerDto.Compare(studentResponse?.StudentDto!, modelDto, out var differences);
            WriteOnConsoleDifferencesIfNotEqual(differences);
            return resultCompare;
        }
        private bool CompareListOfStudentDtoReceivedByListExpected(List<GetStudentsDto> modelAllDto)
        {
            var comparerDto = new ObjectsComparer.Comparer<List<GetStudentsDto>>();
            var studentsResponse = _studentResponse as GetStudentsQueryResponse;
            bool resultCompare = comparerDto.Compare(studentsResponse?.StudentsDto!, modelAllDto, out var differences);
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

        private GetStudentQueryHandler InitGetStudentQueryHandler(long? id)
        {
            var mockResponseFactory = new Mock<IResponseFactory<GetStudentQueryResponse>>();
            _mockStudentRepo.Setup(x => x.GetByIdWithIncludeAsync(It.IsAny<Expression<Func<Student, bool>>>(),
                It.IsAny<Expression<Func<Student, object>>>())).ReturnsAsync(InitStudentEntity(id));
            mockResponseFactory.Setup(x => x.CreateResponse()).Returns(new GetStudentQueryResponse());
            return new GetStudentQueryHandler(_mapper, _mockStudentRepo.Object, mockResponseFactory.Object);
        }

        private GetStudentsQueryHandler InitGetStudentsQueryHandler(bool isListExpected)
        {
            var mockResponseFactory = new Mock<IResponseFactory<GetStudentsQueryResponse>>();
            _mockStudentRepo.Setup(x => x.GetDbSetQueryable()).Returns(isListExpected ? InitListOfStudentEntity().BuildMock() : null);
            mockResponseFactory.Setup(x => x.CreateResponse()).Returns(new GetStudentsQueryResponse());
            return new GetStudentsQueryHandler(_mapper, _mockStudentRepo.Object, mockResponseFactory.Object);
        }
        private GetStudentsQueryResponse InitGetStudentsQueryResponse()
        {
            return new GetStudentsQueryResponse()
            {
                StudentsDto = _mapper.Map<List<GetStudentsDto>>(_allStudentDto)
            };
        }
        private GetStudentQueryResponse InitGetStudentQueryResponse()
        {
            return new GetStudentQueryResponse()
            {
                StudentDto = _mapper.Map<GetStudentDto>(_studentDto)
            };
        }
        private static List<GetStudentDto>? InitListOfStudentDto(bool isExpected)
        {
            if (isExpected)
                return new List<GetStudentDto>()
                {
                    new GetStudentDto()
                    {
                        Id = 3,
                        FirstName = "Test",
                        LastName = "Test",
                        Adress = "MyAdress",
                        Age = 10,
                        SchoolId = 5
                    },
                    new GetStudentDto()
                    {
                        Id = 6,
                        FirstName = "Test6",
                        LastName = "Test6",
                        Adress = "MyAdress6",
                        Age = 16,
                        SchoolId = 10
                    },
                };
            return null;
        }

        private static List<Student>? InitListOfStudentEntity()
        {
            return new List<Student>()
                {
                    new Student(3, "Test", "Test", 10, "MyAdress", 5),
                    new Student(6, "Test6", "Test6", 16, "MyAdress6", 10)
                };
        }
        private static GetStudentDto? InitStudentDto(long? id)
        {
            if (id == 3)
                return new GetStudentDto()
                {
                    Id = 3,
                    FirstName = "Test",
                    LastName = "Test",
                    Adress = "MyAdress",
                    Age = 10,
                    SchoolId = 5
                };
            return null;
        }
        private static Student? InitStudentEntity(long? id)
        {
            if (id == 3)
                return new Student(3, "Test", "Test", 10, "MyAdress", 5);
            return null;
        }
    }
}
