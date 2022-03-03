using SchoolProject.Management.Application.Features.Dto;

namespace SchoolProject.Management.Application.Features.Schools.Queries.GetSchools
{
    public class GetSchoolsDto : SchoolDto
    {
        public bool Haschildren { get; set; }
        public string? Parentname { get; set; }
    }
}
