using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
namespace SchoolProject.Management.Application.Features.Students.Commands.CreateStudent
{
    public class CreateStudentCommand : IRequest<CreateStudentCommandResponse>
    {
        public CreateStudentDto Student { get; set; }
    }
}
