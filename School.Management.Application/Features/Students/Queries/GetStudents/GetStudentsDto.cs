using SchoolProject.Management.Application.Features.Dto;

namespace SchoolProject.Management.Application.Features.Students.Queries.GetStudents
{
    public class GetStudentsDto : StudentDto, IBaseDto
    {
        public long Id { get; set; }
        public bool Haschildren { get; set; }
        public string Parentname { get; set; }
    }
}
