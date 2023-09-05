using ManagementProject.Application.Contracts.MediatR.Command;

namespace ManagementProject.Application.Features.Students.Commands.DeleteStudent
{
    public class DeleteStudentCommand : ICommand<DeleteStudentCommandResponse>
    {
        public long StudentId { get; set; }
    }
}
