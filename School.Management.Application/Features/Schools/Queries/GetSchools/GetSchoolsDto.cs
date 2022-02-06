using SchoolProject.Management.Application.Features.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolProject.Management.Application.Features.Schools.Queries.GetSchools
{
    public class GetSchoolsDto : SchoolDto, IBaseDto
    {
        public long Id { get; set; }
        public bool Haschildren { get; set; }
        public string Parentname { get; set; }
    }
}
