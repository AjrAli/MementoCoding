using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ObjectsComparer;
using SchoolProject.Management.Api.Controllers;
using SchoolProject.Management.Application.Features.Students.Queries.GetStudent;
using Serilog;
using Serilog.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolProject.Management.Api.Tests.Controllers
{
    [TestClass]
    public class StudentControllerTest
    {
        [TestMethod]
        public async Task CheckIfGetStudentReturnCorrectStudent()
        {
            //Arrange
            var serilogLogger = new LoggerConfiguration()
                                .WriteTo.Debug()
                                .CreateLogger();
            var studentMicrosoftLogger = new SerilogLoggerFactory(serilogLogger).CreateLogger<StudentController>();
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.Is<GetStudentQuery>(x => x.StudentId == 3), It.IsAny<CancellationToken>())).ReturnsAsync(InitStudentResponse());
            var studentControllerTest = new StudentController(mediatorMock.Object, studentMicrosoftLogger);

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
            Assert.IsTrue(comparerDto.Compare(InitStudentResponse()?.StudentDto!, modelDto));
        }


        private GetStudentQueryResponse InitStudentResponse()
        {
            return new GetStudentQueryResponse()
            {
                StudentDto = new GetStudentDto()
                {
                    Id = 3,
                    FirstName = "Test",
                    LastName = "Test",
                    Adress = "MyAdress",
                    Age = 10,
                    Haschildren = false,
                    Parentname = "TestSchool",
                    SchoolId = 5
                }
            };
        }
    }
}
