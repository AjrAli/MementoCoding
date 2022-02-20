using MediatR;

namespace SchoolProject.Management.Application.Features.Schools.Commands.UpdateSchool
{
    public class UpdateSchoolCommand : IRequest<UpdateSchoolCommandResponse>
    {
        public UpdateSchoolDto School { get; set; }
    }
}
