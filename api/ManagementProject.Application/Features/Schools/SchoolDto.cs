using ManagementProject.Application.Features.Dto;

namespace ManagementProject.Application.Features.Schools
{
    public record SchoolDto : IBaseDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Adress { get; set; }
        public string? Town { get; set; }
        public string? Description { get; set; }
    }
}
