using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ManagementProject.Application.Exceptions;
using ManagementProject.Application.Features.Schools;
using ManagementProject.Application.Features.Schools.Commands.CreateSchool;
using ManagementProject.Application.Features.Schools.Commands.DeleteSchool;
using ManagementProject.Application.Features.Schools.Commands.UpdateSchool;
using ManagementProject.Application.Features.Schools.Queries.GetSchool;
using ManagementProject.Application.Features.Schools.Queries.GetSchools;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ManagementProject.Api.Controllers.Queries
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class SchoolQueryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SchoolQueryController> _logger;
        public SchoolQueryController(IMediator mediator,
                                ILogger<SchoolQueryController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("{schoolId}")]
        public async Task<IActionResult> GetSchool(long? schoolId)
        {
            GetSchoolQueryResponse? dataReponse = await _mediator.Send(new GetSchoolQuery
            {
                SchoolId = schoolId
            });
            return Ok(dataReponse);
        }
        [HttpGet]
        [Route("{skip?}/{take?}")]
        public async Task<IActionResult> GetSchools(int skip = 0, int take = 0)
        {
            GetSchoolsQueryResponse? dataReponse = await _mediator.Send(new GetSchoolsQuery()
            {
                Skip = skip,
                Take = take
            });
            return Ok(dataReponse);
        }
    }
}
