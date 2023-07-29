using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ManagementProject.Application.Exceptions;
using ManagementProject.Application.Features.Schools;
using ManagementProject.Application.Features.Schools.Commands.CreateSchool;
using ManagementProject.Application.Features.Schools.Commands.DeleteSchool;
using ManagementProject.Application.Features.Schools.Commands.UpdateSchool;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ManagementProject.Api.Controllers.Commands
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class SchoolCommandController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SchoolCommandController> _logger;
        public SchoolCommandController(IMediator mediator,
                                ILogger<SchoolCommandController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }


        [HttpPost]
        [Route("CreateSchool")]
        public async Task<IActionResult> CreateSchool([FromBody] SchoolDto createSchoolDto)
        {
            CreateSchoolCommandResponse? dataReponse = await _mediator.Send(new CreateSchoolCommand
            {
                School = createSchoolDto
            });

            return Ok(dataReponse);
        }

        [HttpPost]
        [Route("DeleteSchool")]
        public async Task<IActionResult> DeleteSchool([FromBody] long schoolId)
        {
            DeleteSchoolCommandResponse? dataReponse = await _mediator.Send(new DeleteSchoolCommand
            {
                SchoolId = schoolId
            });

            return Ok(dataReponse);
        }
        [HttpPost]
        [Route("UpdateSchool")]
        public async Task<IActionResult> UpdateSchool([FromBody] SchoolDto updateSchoolDto)
        {
            UpdateSchoolCommandResponse? dataReponse = await _mediator.Send(new UpdateSchoolCommand
            {
                School = updateSchoolDto
            });
            return Ok(dataReponse);
        }
    }
}
