using ManagementProject.Application.Features.Dto;

namespace ManagementProject.Application.Features.Schools.Queries.GetSchools
{
    public class GetSchoolsDto : SchoolDto
    {
        public bool Haschildren { get; set; }
        public string? Parentname { get; set; }
    }
}
