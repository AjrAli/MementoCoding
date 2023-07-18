using ManagementProject.Application.Features.Dto;

namespace ManagementProject.Application.Features.Students.Queries.GetStudents
{
    public class GetStudentsDto : StudentDto
    {
        public bool Haschildren { get; set; }
        public string? Parentname { get; set; }
    }
}
