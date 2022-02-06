using SchoolProject.Management.Application.Features.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolProject.Management.Application.Features.Students.Queries.GetStudents
{
    public class GetStudentsQueryResponse : BaseResponse
    {
        public List<GetStudentsDto> StudentsDto { get; set; }
        public GetStudentsQueryResponse() : base()
        {

        }
    }
}
