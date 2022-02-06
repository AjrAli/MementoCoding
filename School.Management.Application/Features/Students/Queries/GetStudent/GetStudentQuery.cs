using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolProject.Management.Application.Features.Students.Queries.GetStudent
{
    public class GetStudentQuery : IRequest<GetStudentQueryResponse>
    {
        public long? StudentId { get; set; }
    }
}
