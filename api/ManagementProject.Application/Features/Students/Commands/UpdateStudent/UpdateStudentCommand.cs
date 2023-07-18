using MediatR;

namespace ManagementProject.Application.Features.Students.Commands.UpdateStudent
{
    public class UpdateStudentCommand : IRequest<UpdateStudentCommandResponse>
    {
        public StudentDto? Student { get; set; }
    }
}
