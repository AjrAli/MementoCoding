using MediatR;
namespace SchoolProject.Management.Application.Features.Students.Commands.CreateStudent
{
    public class CreateStudentCommand : IRequest<CreateStudentCommandResponse>
    {
        public StudentDto? Student { get; set; }
    }
}
