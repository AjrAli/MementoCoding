﻿using MediatR;
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

namespace SchoolProject.Management.Api.Controllers.Queries
{
    [ApiController]
    [Route("[controller]")]
    public class StudentQueryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<StudentQueryController> _logger;
        public StudentQueryController(IMediator mediator,
                                ILogger<StudentQueryController> logger)
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
    }
}