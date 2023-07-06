using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolProject.Management.Application.Exceptions;
using SchoolProject.Management.Application.Features.Schools;
using SchoolProject.Management.Application.Features.Schools.Commands.CreateSchool;
using SchoolProject.Management.Application.Features.Schools.Commands.DeleteSchool;
using SchoolProject.Management.Application.Features.Schools.Commands.UpdateSchool;
using SchoolProject.Management.Application.Features.Schools.Queries.GetSchool;
using SchoolProject.Management.Application.Features.Schools.Queries.GetSchools;
using System;
using System.Threading.Tasks;

namespace SchoolProject.Management.Api.Controllers.Queries
{
    [ApiController]
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
            GetSchoolQueryResponse? dataReponse = null;
            try
            {
                dataReponse = await _mediator.Send(new GetSchoolQuery
                {
                    SchoolId = schoolId
                });
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                if (dataReponse?.SchoolDto == null)
                    return NotFound();
            }
            return Ok(dataReponse);
        }
        [HttpGet]
        [Route("{skip?}/{take?}")]
        public async Task<IActionResult> GetSchools(int skip = 0, int take = 0)
        {
            GetSchoolsQueryResponse? dataReponse = null;
            try
            {
                dataReponse = await _mediator.Send(new GetSchoolsQuery()
                {
                    Skip = skip,
                    Take = take
                });
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                if (dataReponse?.SchoolsDto == null)
                    return NotFound();
            }
            return Ok(dataReponse);
        }
    }
}
