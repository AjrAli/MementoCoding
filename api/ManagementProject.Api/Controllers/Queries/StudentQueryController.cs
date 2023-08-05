using ManagementProject.Application.Features.Students.Queries.GetStudent;
using ManagementProject.Application.Features.Students.Queries.GetStudents;
using ManagementProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ManagementProject.Api.Controllers.Queries
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class StudentQueryController : ODataController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<StudentQueryController> _logger;
        public StudentQueryController(IMediator mediator,
                                ILogger<StudentQueryController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("{studentId}")]
        public async Task<IActionResult> GetStudent(long? studentId)
        {
            GetStudentQueryResponse? dataReponse = await _mediator.Send(new GetStudentQuery
            {
                StudentId = studentId
            });

            return Ok(dataReponse);
        }

        [HttpGet]
        public async Task<IActionResult> GetStudents(ODataQueryOptions<Student>? options = null)
        {
            GetStudentsQueryResponse? dataReponse = await _mediator.Send(new GetStudentsQuery()
            {
                Options = options
            });

            return Ok(dataReponse);
        }
    }
}
