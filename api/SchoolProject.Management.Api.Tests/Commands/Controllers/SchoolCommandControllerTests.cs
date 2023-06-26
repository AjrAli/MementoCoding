using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockQueryable.Moq;
using Moq;
using ObjectsComparer;
using SchoolProject.Management.Application.Contracts.Persistence;
using SchoolProject.Management.Application.Features.Response;
using SchoolProject.Management.Application.Profiles.Schools;
using SchoolProject.Management.Domain.Entities;
using Serilog;
using Serilog.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolProject.Management.Api.Tests.Commands.Controllers
{
    [TestClass]
    public partial class SchoolCommandControllerTests
    {
        private readonly ILogger<SchoolCommandControllerTests> _logger = new SerilogLoggerFactory(new LoggerConfiguration()
                                                                                          .WriteTo.Debug()
                                                                                          .CreateLogger())
                                                                  .CreateLogger<SchoolCommandControllerTests>();
        private readonly IMapper _mapper = new MapperConfiguration(x => x.AddProfile<SchoolMappingProfile>()).CreateMapper();
        private IBaseResponse? _schoolResponse;
        private static TestContext? _testContext;
        private Mock<ISchoolRepository> _mockSchoolRepo = new Mock<ISchoolRepository>();
        private Mock<IStudentRepository> _mockStudentRepo = new Mock<IStudentRepository>();

        [ClassInitialize]
        public static void SetupTests(TestContext testContext)
        {
            _testContext = testContext;
        }
    }
}
