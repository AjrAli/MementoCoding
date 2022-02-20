using MediatR;

namespace SchoolProject.Management.Application.Features.Schools.Commands.DeleteSchool
{
    public class DeleteSchoolCommand : IRequest<DeleteSchoolCommandResponse>
    {
        public long? SchoolId { get; set; }
    }
}
