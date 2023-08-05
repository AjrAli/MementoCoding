using ManagementProject.Application.Features.Schools.Queries.GetSchool;
using ManagementProject.Application.Features.Schools.Queries.GetSchools;
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
    public class SchoolQueryController : ODataController
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
        public async Task<IActionResult> GetSchools(ODataQueryOptions<School>? options = null)
        {
            GetSchoolsQueryResponse? dataReponse = await _mediator.Send(new GetSchoolsQuery()
            {
                Options = options
            });
            return Ok(dataReponse);
        }
    }
}
