﻿using AutoMapper;
using ManagementProject.Api.Controllers.Queries;
using ManagementProject.Application.Contracts.Persistence;
using ManagementProject.Application.Exceptions;
using ManagementProject.Application.Features.Response;
using ManagementProject.Application.Features.Students.Queries.GetStudent;
using ManagementProject.Application.Features.Students.Queries.GetStudents;
using ManagementProject.Application.Profiles.Students;
using ManagementProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockQueryable.NSubstitute;
using NSubstitute;
using ObjectsComparer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace ManagementProject.Api.Tests.Unit_Test.Queries.Controllers
{
    [TestClass]
    public class StudentQueryControllerTests
    {
        private readonly ILogger<StudentQueryController> _logger = Substitute.For<ILogger<StudentQueryController>>();
        private readonly IMapper _mapper = new MapperConfiguration(x => x.AddProfile<StudentMappingProfile>()).CreateMapper();
        private GetStudentDto? _studentDto;
        private List<GetStudentDto>? _allStudentDto;
        private IBaseResponse? _studentResponse;
        private static TestContext? _testContext;
        private readonly IMediator _mediatorMock = Substitute.For<IMediator>();
        private readonly IStudentRepository _mockStudentRepo = Substitute.For<IStudentRepository>();

        [ClassInitialize]
        public static void SetupTests(TestContext testContext)
        {
            _testContext = testContext;
        }

        [TestMethod]
        public async Task GetStudent_ReturnNotFoundException()
        {
            //Arrange
            long studentIdRequested = 0;
            SetupGetStudentQueryResponse(studentIdRequested);
            var studentControllerTest = new StudentQueryController(_mediatorMock, _logger);

            // Act && Assert
            await Assert.ThrowsExceptionAsync<NotFoundException>(async () =>
            {
                await studentControllerTest.GetStudent(studentIdRequested);
            });
        }

        [TestMethod]
        public async Task GetStudents_ReturnNotFoundException()
        {
            //Arrange
            SetupGetStudentsQueryResponse(false);
            var studentControllerTest = new StudentQueryController(_mediatorMock, _logger);

            // Act && Assert
            await Assert.ThrowsExceptionAsync<NotFoundException>(async () =>
            {
                await studentControllerTest.GetStudents();
            });
        }

        [TestMethod]
        public async Task GetStudent_ReturnCorrectStudentDto()
        {
            //Arrange
            long studentIdRequested = 3;
            SetupGetStudentQueryResponse(studentIdRequested);
            var studentControllerTest = new StudentQueryController(_mediatorMock, _logger);

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
            SetupGetStudentsQueryResponse(true);
            var studentControllerTest = new StudentQueryController(_mediatorMock, _logger);

            //Act
            var resultStudentCall = await studentControllerTest.GetStudents();

            //Assert
            var modelAllDto = (((resultStudentCall as OkObjectResult)?.Value) as GetStudentsQueryResponse)?.StudentsDto;
            Assert.IsNotNull(modelAllDto);
            bool resultCompare = CompareListOfStudentDtoReceivedByListExpected(modelAllDto);
            Assert.IsTrue(resultCompare);
        }

        private void SetupGetStudentsQueryResponse(bool isListExpected)
        {
            _allStudentDto = InitListOfStudentDto(isListExpected);
            _studentResponse = InitGetStudentsQueryResponse();
            _mediatorMock.Send(Arg.Any<GetStudentsQuery>(), default).Returns(x =>
            {
                return InitGetStudentsQueryHandler(isListExpected).Handle(x.Arg<GetStudentsQuery>(), x.Arg<CancellationToken>());
            });
        }

        private void SetupGetStudentQueryResponse(long? id)
        {
            _studentDto = InitStudentDto(id);
            _studentResponse = InitGetStudentQueryResponse();
            _mediatorMock.Send(Arg.Any<GetStudentQuery>(), default).Returns(x =>
            {
                return InitGetStudentQueryHandler(id).Handle(x.Arg<GetStudentQuery>(), x.Arg<CancellationToken>());
            });

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
            _mockStudentRepo.GetByIdWithIncludeAsync(Arg.Any<Expression<Func<Student, bool>>>(), Arg.Any<Expression<Func<Student, object>>>()).Returns(InitStudentEntity(id));
            return new GetStudentQueryHandler(_mapper, _mockStudentRepo);
        }

        private GetStudentsQueryHandler InitGetStudentsQueryHandler(bool isListExpected)
        {

            _mockStudentRepo.GetDbSetQueryable().Returns(isListExpected ? InitListOfStudentEntity().BuildMock() : null);
            _mockStudentRepo.CountAsync().Returns(isListExpected ? InitListOfStudentEntity().Count : 0);
            return new GetStudentsQueryHandler(_mapper, _mockStudentRepo);
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
