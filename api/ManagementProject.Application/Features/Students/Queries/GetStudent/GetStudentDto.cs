using ManagementProject.Application.Features.Dto;

namespace ManagementProject.Application.Features.Students.Queries.GetStudent
{
    public record GetStudentDto : StudentDto
    {
        public bool Haschildren { get; set; }
        public string? Parentname { get; set; }
    }
}
