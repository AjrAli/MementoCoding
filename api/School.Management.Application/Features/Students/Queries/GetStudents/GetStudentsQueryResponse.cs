using SchoolProject.Management.Application.Features.Response;
using System.Collections.Generic;

namespace SchoolProject.Management.Application.Features.Students.Queries.GetStudents
{
    public class GetStudentsQueryResponse : BaseResponse
    {
        public List<GetStudentsDto>? StudentsDto { get; set; }
        public GetStudentsQueryResponse() : base()
        {

        }
    }
}
