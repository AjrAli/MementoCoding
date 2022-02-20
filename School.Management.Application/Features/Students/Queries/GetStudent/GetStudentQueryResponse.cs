using SchoolProject.Management.Application.Features.Response;

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
