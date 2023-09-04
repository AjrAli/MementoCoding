using ManagementProject.Application.Contracts.MediatR.Command;

namespace ManagementProject.Application.Features.Schools.Commands.CreateSchool
{
    public class CreateSchoolCommand : ICommand<CreateSchoolCommandResponse>
    {
        public SchoolDto? School { get; set; }
    }
}
