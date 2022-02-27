using SchoolProject.Management.Application.Features.Dto;

namespace SchoolProject.Management.Application.Features.Schools.Queries.GetSchool
{
    public class GetSchoolDto : SchoolDto, IBaseDto
    {
        public long Id { get; set; }
        public bool Haschildren { get; set; }
        public string? Parentname { get; set; }
    }
}
