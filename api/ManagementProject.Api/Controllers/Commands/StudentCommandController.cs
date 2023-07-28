using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ManagementProject.Application.Exceptions;
using ManagementProject.Application.Features.Students;
using ManagementProject.Application.Features.Students.Commands.CreateStudent;
using ManagementProject.Application.Features.Students.Commands.DeleteStudent;
using ManagementProject.Application.Features.Students.Commands.UpdateStudent;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ManagementProject.Api.Controllers.Commands
{
    [ApiController]
    [Authorize]
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
            CreateStudentCommandResponse? dataReponse = await _mediator.Send(new CreateStudentCommand
            {
                Student = createStudentDto
            });

            return Ok(dataReponse);
        }

        [HttpPost]
        [Route("DeleteStudent")]
        public async Task<IActionResult> DeleteStudent([FromBody] long studentId)
        {
            DeleteStudentCommandResponse? dataReponse = await _mediator.Send(new DeleteStudentCommand
            {
                StudentId = studentId
            });

            return Ok(dataReponse);

        }
        [HttpPost]
        [Route("UpdateStudent")]
        public async Task<IActionResult> UpdateStudent([FromBody] StudentDto updateStudentDto)
        {
            UpdateStudentCommandResponse? dataReponse = await _mediator.Send(new UpdateStudentCommand
            {
                Student = updateStudentDto
            });

            return Ok(dataReponse);
        }
    }
}
