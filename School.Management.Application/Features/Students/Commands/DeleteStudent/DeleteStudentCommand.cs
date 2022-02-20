using MediatR;

namespace SchoolProject.Management.Application.Features.Students.Commands.DeleteStudent
{
    public class DeleteStudentCommand : IRequest<DeleteStudentCommandResponse>
    {
        public long? StudentId { get; set; }
    }
}
