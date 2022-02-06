using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
namespace SchoolProject.Management.Application.Features.Schools.Commands.CreateSchool
{
    public class CreateSchoolCommand : IRequest<CreateSchoolCommandResponse>
    {
        public CreateSchoolDto School { get; set; }
    }
}
