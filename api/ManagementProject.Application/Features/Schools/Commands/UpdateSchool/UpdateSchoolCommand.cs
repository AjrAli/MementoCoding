using MediatR;

namespace ManagementProject.Application.Features.Schools.Commands.UpdateSchool
{
    public class UpdateSchoolCommand : IRequest<UpdateSchoolCommandResponse>
    {
        public SchoolDto? School { get; set; }
    }
}
