using ManagementProject.Application.Contracts.MediatR.Command;

namespace ManagementProject.Application.Features.Schools.Commands.DeleteSchool
{
    public class DeleteSchoolCommand : ICommand<DeleteSchoolCommandResponse>
    {
        public long SchoolId { get; set; }
    }
}
