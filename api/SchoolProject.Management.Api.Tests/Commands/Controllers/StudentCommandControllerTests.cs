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
using SchoolProject.Management.Application.Profiles.Students;
using SchoolProject.Management.Domain.Entities;
using Serilog;
using Serilog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolProject.Management.Api.Tests.Commands.Controllers
{
    [TestClass]
    public partial class StudentCommandControllerTests
    {

        private readonly ILogger<StudentCommandControllerTests> _logger = new SerilogLoggerFactory(new LoggerConfiguration()
                                                                                          .WriteTo.Debug()
                                                                                          .CreateLogger())
                                                                  .CreateLogger<StudentCommandControllerTests>();
        private readonly IMapper _mapper = new MapperConfiguration(x => x.AddProfile<StudentMappingProfile>()).CreateMapper();
        private IBaseResponse? _studentResponse;
        private static TestContext? _testContext;
        private Mock<IStudentRepository> _mockStudentRepo = new Mock<IStudentRepository>();

        [ClassInitialize]
        public static void SetupTests(TestContext testContext)
        {
            _testContext = testContext;
        }
       
    }
}
