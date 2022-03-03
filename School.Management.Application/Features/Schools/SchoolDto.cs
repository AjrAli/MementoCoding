using SchoolProject.Management.Application.Features.Dto;

namespace SchoolProject.Management.Application.Features.Schools
{
    public class SchoolDto : IBaseDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Adress { get; set; }
        public string? Town { get; set; }
        public string? Description { get; set; }
    }
}
