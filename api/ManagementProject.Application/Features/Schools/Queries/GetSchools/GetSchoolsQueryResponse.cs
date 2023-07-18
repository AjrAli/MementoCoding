using ManagementProject.Application.Features.Response;
using System.Collections.Generic;

namespace ManagementProject.Application.Features.Schools.Queries.GetSchools
{
    public class GetSchoolsQueryResponse : BaseResponse
    {
        public List<GetSchoolsDto>? SchoolsDto { get; set; }
        public GetSchoolsQueryResponse() : base()
        {

        }
    }
}
