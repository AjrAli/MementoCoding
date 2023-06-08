using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolProject.Management.Application.Exceptions;
using SchoolProject.Management.Application.Features.Students;
using SchoolProject.Management.Application.Features.Students.Commands.CreateStudent;
using SchoolProject.Management.Application.Features.Students.Commands.DeleteStudent;
using SchoolProject.Management.Application.Features.Students.Commands.UpdateStudent;
using SchoolProject.Management.Application.Features.Students.Queries.GetStudent;
using SchoolProject.Management.Application.Features.Students.Queries.GetStudents;
using System;
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
            GetStudentQueryResponse? dataReponse = null;
            try
            {
                dataReponse = await _mediator.Send(new GetStudentQuery
                {
                    StudentId = studentId
                });
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                if (dataReponse?.StudentDto == null)
                    return NotFound();
            }
            return Ok(dataReponse);
        }

        [HttpGet]
        [Route("{skip?}/{take?}")]
        public async Task<IActionResult> GetStudents(int skip = 0, int take = 0)
        {
            GetStudentsQueryResponse? dataReponse = null;
            try
            {
                dataReponse = await _mediator.Send(new GetStudentsQuery()
                {
                    Skip = skip,
                    Take = take
                });
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                if (dataReponse?.StudentsDto == null)
                    return NotFound();
            }
            return Ok(dataReponse);
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
