using SchoolProject.Management.Application.Features.Response;
using System;
using System.Collections.Generic;
using System.Text;

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
