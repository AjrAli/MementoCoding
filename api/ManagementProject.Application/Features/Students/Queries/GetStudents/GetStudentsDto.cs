using ManagementProject.Application.Features.Dto;

namespace ManagementProject.Application.Features.Students.Queries.GetStudents
{
    public record GetStudentsDto : StudentDto
    {
        public bool Haschildren { get; set; }
        public string? Parentname { get; set; }
    }
}
