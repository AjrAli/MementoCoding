using ManagementProject.Application.Contracts.MediatR.Command;

namespace ManagementProject.Application.Features.Students.Commands.UpdateStudent
{
    public class UpdateStudentCommand : ICommand<UpdateStudentCommandResponse>
    {
        public StudentDto? Student { get; set; }
    }
}
