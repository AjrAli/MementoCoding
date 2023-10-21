using ManagementProject.Application.Features.Dto;

namespace ManagementProject.Application.Features.Schools.Queries.GetSchools
{
    public record GetSchoolsDto : SchoolDto
    {
        public bool Haschildren { get; set; }
        public string? Parentname { get; set; }
    }
}
