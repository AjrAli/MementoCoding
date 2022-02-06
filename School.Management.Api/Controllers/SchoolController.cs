using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolProject.Management.Application.Features.Schools.Commands.CreateSchool;
using SchoolProject.Management.Application.Features.Schools.Commands.DeleteSchool;
using SchoolProject.Management.Application.Features.Schools.Commands.UpdateSchool;
using SchoolProject.Management.Application.Features.Schools.Queries.GetSchool;
using SchoolProject.Management.Application.Features.Schools.Queries.GetSchools;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolProject.Management.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SchoolController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SchoolController> _logger;
        public SchoolController(IMediator mediator,
                                ILogger<SchoolController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        
        [HttpGet]
        [Route("{SchoolId}")]
        public async Task<IActionResult> GetSchool(long? schoolId)
        {
            var data = await _mediator.Send(new GetSchoolQuery
            {
                SchoolId = schoolId
            });
            if (data.SchoolDto == null)
                return NotFound();
            return Ok(data);
        }
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetSchools()
        {
            return Ok(await _mediator.Send(new GetSchoolsQuery()));
        }


        [HttpPost]
        [Route("CreateSchool")]
        public async Task<IActionResult> CreateSchool([FromBody] CreateSchoolDto createSchoolDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("Provided model is not valid");
                return BadRequest(ModelState);
            }

            return Ok(await _mediator.Send(new CreateSchoolCommand
            {
                School = createSchoolDto
            }));

        }

        [HttpPost]
        [Route("DeleteSchool")]
        public async Task<IActionResult> DeleteSchool([FromBody] long schoolId)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("Provided model is not valid");
                return BadRequest(ModelState);
            }

            return Ok(await _mediator.Send(new DeleteSchoolCommand
            {
                SchoolId = schoolId
            }));

        }
        [HttpPost]
        [Route("UpdateSchool")]
        public async Task<IActionResult> UpdateSchool([FromBody] UpdateSchoolDto updateSchoolDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("Provided model is not valid");
                return BadRequest(ModelState);
            }
            return Ok(await _mediator.Send(new UpdateSchoolCommand
            {
                School = updateSchoolDto
            }));

        }
    }
}
