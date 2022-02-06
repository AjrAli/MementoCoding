using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace SchoolProject.Management.Application.Features.Students.Commands.UpdateStudent
{
    public class UpdateStudentCommand : IRequest<UpdateStudentCommandResponse>
    {
        public UpdateStudentDto Student { get; set; }
    }
}
