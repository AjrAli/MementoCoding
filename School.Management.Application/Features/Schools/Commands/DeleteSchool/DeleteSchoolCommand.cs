using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolProject.Management.Application.Features.Schools.Commands.DeleteSchool
{
    public class DeleteSchoolCommand : IRequest<DeleteSchoolCommandResponse>
    {
        public long? SchoolId { get; set; }
    }
}
