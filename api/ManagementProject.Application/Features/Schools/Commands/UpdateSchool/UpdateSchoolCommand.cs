using ManagementProject.Application.Contracts.MediatR.Command;

namespace ManagementProject.Application.Features.Schools.Commands.UpdateSchool
{
    public class UpdateSchoolCommand : ICommand<UpdateSchoolCommandResponse>
    {
        public SchoolDto? School { get; set; }
    }
}
