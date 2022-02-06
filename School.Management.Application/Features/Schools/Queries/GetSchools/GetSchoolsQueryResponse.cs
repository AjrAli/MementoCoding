using SchoolProject.Management.Application.Features.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolProject.Management.Application.Features.Schools.Queries.GetSchools
{
    public class GetSchoolsQueryResponse : BaseResponse
    {
        public List<GetSchoolsDto> SchoolsDto { get; set; }
        public GetSchoolsQueryResponse() : base()
        {

        }
    }
}
