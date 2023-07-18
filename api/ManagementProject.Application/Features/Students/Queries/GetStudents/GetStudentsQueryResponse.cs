using ManagementProject.Application.Features.Response;
using System.Collections.Generic;

namespace ManagementProject.Application.Features.Students.Queries.GetStudents
{
    public class GetStudentsQueryResponse : BaseResponse
    {
        public List<GetStudentsDto>? StudentsDto { get; set; }
        public GetStudentsQueryResponse() : base()
        {

        }
    }
}
