using SchoolProject.Management.Application.Features.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolProject.Management.Application.Features.Students.Queries.GetStudent
{
    public class GetStudentQueryResponse : BaseResponse
    {
        public GetStudentDto StudentDto { get; set; }
        public GetStudentQueryResponse() : base()
        {

        }
    }
}
