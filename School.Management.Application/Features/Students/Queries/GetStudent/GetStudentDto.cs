using SchoolProject.Management.Application.Features.Dto;

namespace SchoolProject.Management.Application.Features.Students.Queries.GetStudent
{
    public class GetStudentDto : StudentDto
    {
        public bool Haschildren { get; set; }
        public string? Parentname { get; set; }
    }
}
