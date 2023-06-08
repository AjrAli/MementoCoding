using MediatR;

namespace SchoolProject.Management.Application.Features.Schools.Queries.GetSchools
{
    public class GetSchoolsQuery : IRequest<GetSchoolsQueryResponse>
    {
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}
