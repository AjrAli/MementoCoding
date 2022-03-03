using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolProject.Management.Application.Features.Students.Commands.CreateStudent;
using SchoolProject.Management.Application.Features.Students.Commands.DeleteStudent;
using SchoolProject.Management.Application.Features.Students.Commands.UpdateStudent;
using SchoolProject.Management.Application.Features.Students.Queries.GetStudent;
using SchoolProject.Management.Application.Features.Students.Queries.GetStudents;
using System.Threading.Tasks;

namespace SchoolProject.Management.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<StudentController> _logger;
        public StudentController(IMediator mediator,
                                ILogger<StudentController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("{studentId}")]
        public async Task<IActionResult> GetStudent(long? studentId)
        {
            var data = await _mediator.Send(new GetStudentQuery
            {
                StudentId = studentId
            });
            if (data?.StudentDto == null)
                return NotFound();
            return Ok(data);
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetStudents()
        {
            return Ok(await _mediator.Send(new GetStudentsQuery()));
        }


        [HttpPost]
        [Route("CreateStudent")]
        public async Task<IActionResult> CreateStudent([FromBody] CreateStudentDto createStudentDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("Provided model is not valid");
                return BadRequest(ModelState);
            }

            return Ok(await _mediator.Send(new CreateStudentCommand
            {
                Student = createStudentDto
            }));

        }

        [HttpPost]
        [Route("DeleteStudent")]
        public async Task<IActionResult> DeleteStudent([FromBody] long studentId)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("Provided model is not valid");
                return BadRequest(ModelState);
            }

            return Ok(await _mediator.Send(new DeleteStudentCommand
            {
                StudentId = studentId
            }));

        }
        [HttpPost]
        [Route("UpdateStudent")]
        public async Task<IActionResult> UpdateStudent([FromBody] UpdateStudentDto updateStudentDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("Provided model is not valid");
                return BadRequest(ModelState);
            }
            return Ok(await _mediator.Send(new UpdateStudentCommand
            {
                Student = updateStudentDto
            }));

        }
    }
}
