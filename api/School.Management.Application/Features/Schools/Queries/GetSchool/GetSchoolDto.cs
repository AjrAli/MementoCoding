using SchoolProject.Management.Application.Features.Dto;
using SchoolProject.Management.Application.Features.Students.Queries.GetStudents;
using System.Collections.Generic;

namespace SchoolProject.Management.Application.Features.Schools.Queries.GetSchool
{
    public class GetSchoolDto : SchoolDto
    {
        public bool Haschildren { get; set; }
        public string? Parentname { get; set; }
        public List<GetStudentsDto> Students { get; set; } = new List<GetStudentsDto>();
    }
}
