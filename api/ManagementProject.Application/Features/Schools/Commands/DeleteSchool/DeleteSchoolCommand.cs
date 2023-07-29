using MediatR;

namespace ManagementProject.Application.Features.Schools.Commands.DeleteSchool
{
    public class DeleteSchoolCommand : IRequest<DeleteSchoolCommandResponse>
    {
        public long SchoolId { get; set; }
    }
}
