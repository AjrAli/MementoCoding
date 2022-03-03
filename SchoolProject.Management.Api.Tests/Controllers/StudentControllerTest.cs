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
using SchoolProject.Management.Application.Features.Students.Queries.GetStudent;
using SchoolProject.Management.Application.Profiles.Students;
using SchoolProject.Management.Domain.Entities;
using Serilog;
using Serilog.Extensions.Logging;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolProject.Management.Api.Tests.Controllers
{
    [TestClass]
    public class StudentControllerTest
    {

        private readonly ILogger<StudentController> _logger = new SerilogLoggerFactory(new LoggerConfiguration()
                                                                                          .WriteTo.Debug()
                                                                                          .CreateLogger())
                                                                  .CreateLogger<StudentController>();
        private IBaseDto? _studentDto;
        private IBaseResponse? _studentResponse;


        [TestMethod]
        public async Task CheckIfGetStudentReturnCorrectStudentDto()
        {
            //Arrange
            _studentDto = InitStudentDto();
            _studentResponse = InitStudentResponse();
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.Is<GetStudentQuery>(x => x.StudentId == 3), default(CancellationToken))).Returns(
                async (GetStudentQuery q, CancellationToken token) =>
                await InitStudentHandler(q.StudentId).Handle(q, token));
            var studentControllerTest = new StudentController(mediatorMock.Object, _logger);

            //Act
            var resultStudentCall = await studentControllerTest.GetStudent(3);

            //Assert
            var okObjectResultFromStudent = resultStudentCall as OkObjectResult;
            Assert.IsNotNull(okObjectResultFromStudent);
            var modelQueryResponse = okObjectResultFromStudent.Value as GetStudentQueryResponse;
            Assert.IsNotNull(modelQueryResponse);
            var modelDto = modelQueryResponse.StudentDto;
            Assert.IsNotNull(modelDto);
            var comparerDto = new Comparer<GetStudentDto>();
            Assert.IsTrue(comparerDto.Compare(((GetStudentQueryResponse)_studentResponse)?.StudentDto!, modelDto));
        }

        private GetStudentQueryHandler InitStudentHandler(long? id)
        {
            //var mockStudentMapper = new Mock<IMapper>();
            //mockStudentMapper.Setup(x => x.Map<GetStudentDto>(It.IsAny<Student>()));
            IMapper mapper = new MapperConfiguration(x => x.AddProfile<StudentMappingProfile>()).CreateMapper();
            var mockStudentRepo = new Mock<IStudentRepository>();
            mockStudentRepo.Setup(x => x.GetByIdWithIncludeAsync(It.IsAny<Expression<Func<Student, bool>>>(),
                It.IsAny<Expression<Func<Student, object>>>())).ReturnsAsync(InitStudentEntity(id));
            return new GetStudentQueryHandler(mapper, mockStudentRepo.Object);
        }

        private GetStudentQueryResponse InitStudentResponse()
        {
            return new GetStudentQueryResponse()
            {
                StudentDto = (GetStudentDto)_studentDto!
            };
        }
        private GetStudentDto InitStudentDto()
        {
            return new GetStudentDto()
            {
                Id = 3,
                FirstName = "Test",
                LastName = "Test",
                Adress = "MyAdress",
                Age = 10,
                Haschildren = false,
                SchoolId = 5
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
