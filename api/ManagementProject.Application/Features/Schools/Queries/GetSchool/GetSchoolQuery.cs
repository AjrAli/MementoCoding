using MediatR;

namespace ManagementProject.Application.Features.Schools.Queries.GetSchool
{
    public class GetSchoolQuery : IRequest<GetSchoolQueryResponse>
    {
        public long? SchoolId { get; set; }
    }
}
