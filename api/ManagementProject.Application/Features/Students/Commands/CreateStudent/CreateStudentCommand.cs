using ManagementProject.Application.Contracts.MediatR.Command;

namespace ManagementProject.Application.Features.Students.Commands.CreateStudent
{
    public class CreateStudentCommand : ICommand<CreateStudentCommandResponse>
    {
        public StudentDto? Student { get; set; }
    }
}
