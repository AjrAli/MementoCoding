using ManagementProject.Application.Features.Response;

namespace ManagementProject.Application.Features.Students.Queries.GetStudent
{
    public class GetStudentQueryResponse : BaseResponse
    {
        public GetStudentDto? StudentDto { get; set; }
        public GetStudentQueryResponse() : base()
        {

        }
    }
}
