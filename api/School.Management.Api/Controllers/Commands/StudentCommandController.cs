using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolProject.Management.Application.Exceptions;
using SchoolProject.Management.Application.Features.Students;
using SchoolProject.Management.Application.Features.Students.Commands.CreateStudent;
using SchoolProject.Management.Application.Features.Students.Commands.DeleteStudent;
using SchoolProject.Management.Application.Features.Students.Commands.UpdateStudent;
using System.Threading.Tasks;

namespace SchoolProject.Management.Api.Controllers.Commands
{
    [ApiController]
    [Route("[controller]")]
    public class StudentCommandController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<StudentCommandController> _logger;
        public StudentCommandController(IMediator mediator,
                                ILogger<StudentCommandController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        [Route("CreateStudent")]
        public async Task<IActionResult> CreateStudent([FromBody] StudentDto createStudentDto)
        {
            CreateStudentCommandResponse? dataReponse;
            try
            {
                dataReponse = await _mediator.Send(new CreateStudentCommand
                {
                    Student = createStudentDto
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
        [Route("DeleteStudent")]
        public async Task<IActionResult> DeleteStudent([FromBody] long studentId)
        {
            DeleteStudentCommandResponse? dataReponse;
            try
            {
                dataReponse = await _mediator.Send(new DeleteStudentCommand
                {
                    StudentId = studentId
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
        [Route("UpdateStudent")]
        public async Task<IActionResult> UpdateStudent([FromBody] StudentDto updateStudentDto)
        {
            UpdateStudentCommandResponse? dataReponse;
            try
            {
                dataReponse = await _mediator.Send(new UpdateStudentCommand
                {
                    Student = updateStudentDto
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
