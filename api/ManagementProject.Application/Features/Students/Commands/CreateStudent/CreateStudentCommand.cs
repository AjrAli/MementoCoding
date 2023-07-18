using MediatR;
namespace ManagementProject.Application.Features.Students.Commands.CreateStudent
{
    public class CreateStudentCommand : IRequest<CreateStudentCommandResponse>
    {
        public StudentDto? Student { get; set; }
    }
}
