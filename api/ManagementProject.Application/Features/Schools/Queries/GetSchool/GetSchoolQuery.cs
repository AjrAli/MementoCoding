using ManagementProject.Application.Contracts.MediatR.Query;

namespace ManagementProject.Application.Features.Schools.Queries.GetSchool
{
    public class GetSchoolQuery : IQuery<GetSchoolQueryResponse>
    {
        public long? SchoolId { get; set; }
    }
}
