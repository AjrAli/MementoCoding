using MediatR;

namespace ManagementProject.Application.Features.Schools.Queries.GetSchools
{
    public class GetSchoolsQuery : IRequest<GetSchoolsQueryResponse>
    {
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}
