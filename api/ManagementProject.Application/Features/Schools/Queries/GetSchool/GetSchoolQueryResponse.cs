using ManagementProject.Application.Features.Response;

namespace ManagementProject.Application.Features.Schools.Queries.GetSchool
{
    public class GetSchoolQueryResponse : BaseResponse
    {
        public GetSchoolDto? SchoolDto { get; set; }
        public GetSchoolQueryResponse() : base()
        {

        }
    }
}
