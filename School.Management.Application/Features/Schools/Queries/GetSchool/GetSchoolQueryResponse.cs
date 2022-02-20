using SchoolProject.Management.Application.Features.Response;

namespace SchoolProject.Management.Application.Features.Schools.Queries.GetSchool
{
    public class GetSchoolQueryResponse : BaseResponse
    {
        public GetSchoolDto SchoolDto { get; set; }
        public GetSchoolQueryResponse() : base()
        {

        }
    }
}
