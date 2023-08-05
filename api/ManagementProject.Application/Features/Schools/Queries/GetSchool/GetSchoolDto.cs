using ManagementProject.Application.Features.Dto;
using ManagementProject.Application.Features.Students.Queries.GetStudents;
using System.Collections.Generic;

namespace ManagementProject.Application.Features.Schools.Queries.GetSchool
{
    public class GetSchoolDto : SchoolDto
    {
        public bool Haschildren { get; set; }
        public string? Parentname { get; set; }
    }
}
