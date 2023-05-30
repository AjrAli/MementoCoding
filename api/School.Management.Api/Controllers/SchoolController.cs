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
        [Route("")]
        public async Task<IActionResult> GetSchools()
        {
            GetSchoolsQueryResponse? dataReponse = null;
            try
            {
                dataReponse = await _mediator.Send(new GetSchoolsQuery());
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                if (dataReponse?.SchoolsDto == null)
                    return NotFound();
            }
            return Ok(dataReponse);
        }


        [HttpPost]
        [Route("CreateSchool")]
        public async Task<IActionResult> CreateSchool([FromBody] SchoolDto createSchoolDto)
        {
            CreateSchoolCommandResponse? dataReponse;
            try
            {
                dataReponse = await _mediator.Send(new CreateSchoolCommand
                {
                    School = createSchoolDto
                });
            }
            catch (BadRequestException ex)
            {
                _logger.LogWarning(ex.Message);
                var errorResponse = ex.CreateErrorResponse();
                return BadRequest(errorResponse);
            }
            return Ok(dataReponse);
        }

        [HttpPost]
        [Route("DeleteSchool")]
        public async Task<IActionResult> DeleteSchool([FromBody] long schoolId)
        {
            DeleteSchoolCommandResponse? dataReponse;
            try
            {
                dataReponse = await _mediator.Send(new DeleteSchoolCommand
                {
                    SchoolId = schoolId
                });
            }
            catch (BadRequestException ex)
            {
                _logger.LogWarning(ex.Message);
                var errorResponse = ex.CreateErrorResponse();
                return BadRequest(errorResponse);
            }
            return Ok(dataReponse);
        }
        [HttpPost]
        [Route("UpdateSchool")]
        public async Task<IActionResult> UpdateSchool([FromBody] SchoolDto updateSchoolDto)
        {
            UpdateSchoolCommandResponse? dataReponse;
            try
            {
                dataReponse = await _mediator.Send(new UpdateSchoolCommand
                {
                    School = updateSchoolDto
                });
            }
            catch (BadRequestException ex)
            {
                _logger.LogWarning(ex.Message);
                var errorResponse = ex.CreateErrorResponse();
                return BadRequest(errorResponse);
            }
            return Ok(dataReponse);
        }
    }
}
