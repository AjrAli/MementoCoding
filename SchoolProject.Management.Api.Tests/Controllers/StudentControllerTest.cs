using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ObjectsComparer;
using SchoolProject.Management.Api.Controllers;
using SchoolProject.Management.Application.Contracts.Persistence;
using SchoolProject.Management.Application.Features.Dto;
using SchoolProject.Management.Application.Features.Response;
using SchoolProject.Management.Application.Features.Students;
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
using System.Linq;

namespace SchoolProject.Management.Api.Tests.Controllers
{
    [TestClass]
    public class StudentControllerTest
    {

        private readonly ILogger<StudentController> _logger = new SerilogLoggerFactory(new LoggerConfiguration()
                                                                                          .WriteTo.Debug()
                                                                                          .CreateLogger())
                                                                  .CreateLogger<StudentController>();
        private readonly IMapper _mapper = new MapperConfiguration(x => x.AddProfile<StudentMappingProfile>()).CreateMapper();
        private StudentDto? _studentDto;
        private List<StudentDto>? _allStudentDto;
        private IBaseResponse? _studentResponse;
        private static TestContext? _testContext;

        [ClassInitialize]
        public static void SetupTests(TestContext testContext)
        {
            _testContext = testContext;
        }
        [TestMethod]
        public async Task CheckIfGetStudentReturnCorrectStudentDto()
        {
            //Arrange
            long studentIdRequested = 3;
            _studentDto = InitStudentDto();
            _studentResponse = InitGetStudentQueryResponse();
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.Is<GetStudentQuery>(x => x.StudentId == studentIdRequested), default(CancellationToken))).Returns(
                async (GetStudentQuery q, CancellationToken token) =>
                await InitGetStudentQueryHandler(q.StudentId).Handle(q, token));
            var studentControllerTest = new StudentController(mediatorMock.Object, _logger);

            //Act
            var resultStudentCall = await studentControllerTest.GetStudent(studentIdRequested);

            //Assert
            var okObjectResultFromStudent = resultStudentCall as OkObjectResult;
            Assert.IsNotNull(okObjectResultFromStudent);
            var modelQueryResponse = okObjectResultFromStudent.Value as GetStudentQueryResponse;
            Assert.IsNotNull(modelQueryResponse);
            var modelDto = modelQueryResponse.StudentDto;
            Assert.IsNotNull(modelDto);
            var comparerDto = new ObjectsComparer.Comparer<GetStudentDto>();
            IEnumerable<Difference> differences;
            bool resultCompare = comparerDto.Compare(((GetStudentQueryResponse)_studentResponse)?.StudentDto!, modelDto, out differences);
            WriteOnConsoleDifferencesIfNotEqual(differences);
            Assert.IsTrue(resultCompare);
        }

        [TestMethod]
        public async Task CheckIfGetStudentsReturnCorrectListOfStudentDto()
        {
            //Arrange
            _allStudentDto = InitListOfStudentDto();
            _studentResponse = InitGetStudentsQueryResponse();
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<GetStudentsQuery>(), default(CancellationToken))).Returns(
                async (GetStudentsQuery q, CancellationToken token) =>
                await InitGetStudentsQueryHandler().Handle(q, token));
            var studentControllerTest = new StudentController(mediatorMock.Object, _logger);

            //Act
            var resultStudentCall = await studentControllerTest.GetStudents();

            //Assert
            var okObjectResultFromStudent = resultStudentCall as OkObjectResult;
            Assert.IsNotNull(okObjectResultFromStudent);
            var modelQueryResponse = okObjectResultFromStudent.Value as GetStudentsQueryResponse;
            Assert.IsNotNull(modelQueryResponse);
            var modelAllDto = modelQueryResponse.StudentsDto;
            Assert.IsNotNull(modelAllDto);
            var comparerDto = new ObjectsComparer.Comparer<List<GetStudentsDto>>();
            IEnumerable<Difference> differences;
            bool resultCompare = comparerDto.Compare(((GetStudentsQueryResponse)_studentResponse)?.StudentsDto!, modelAllDto, out differences);
            WriteOnConsoleDifferencesIfNotEqual(differences);
            Assert.IsTrue(resultCompare);
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
            var mockStudentRepo = new Mock<IStudentRepository>();
            mockStudentRepo.Setup(x => x.GetByIdWithIncludeAsync(It.IsAny<Expression<Func<Student, bool>>>(),
                It.IsAny<Expression<Func<Student, object>>>())).ReturnsAsync(InitStudentEntity(id));
            return new GetStudentQueryHandler(_mapper, mockStudentRepo.Object);
        }

        private GetStudentsQueryHandler InitGetStudentsQueryHandler()
        {
            var mockStudentRepo = new Mock<IStudentRepository>();
            mockStudentRepo.Setup(x => x.GetAllWithIncludeAsync(It.IsAny<Expression<Func<Student, object>>>())).ReturnsAsync(InitListOfStudentEntity());
            return new GetStudentsQueryHandler(_mapper, mockStudentRepo.Object);
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
        private List<StudentDto> InitListOfStudentDto()
        {
            return new List<StudentDto>()
            {
                new StudentDto()
                {
                    Id = 3,
                    FirstName = "Test",
                    LastName = "Test",
                    Adress = "MyAdress",
                    Age = 10,
                    SchoolId = 5
                },
                new StudentDto()
                {
                    Id = 6,
                    FirstName = "Test6",
                    LastName = "Test6",
                    Adress = "MyAdress6",
                    Age = 16,
                    SchoolId = 10
                },
            };
        }
        private StudentDto InitStudentDto()
        {
            return new StudentDto()
            {
                Id = 3,
                FirstName = "Test",
                LastName = "Test",
                Adress = "MyAdress",
                Age = 10,
                SchoolId = 5
            };
        }
        private List<Student> InitListOfStudentEntity()
        {
            return new List<Student>()
            {
                new Student(3, "Test", "Test", 10, "MyAdress", 5),
                new Student(6, "Test6", "Test6", 16, "MyAdress6", 10)
            };
        }
        private Student InitStudentEntity(long? id)
        {
            if (id == 3)
                return new Student(3, "Test", "Test", 10, "MyAdress", 5);
            return new Student();
        }
    }
}
