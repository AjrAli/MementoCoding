using MediatR;

namespace SchoolProject.Management.Application.Features.Schools.Queries.GetSchool
{
    public class GetSchoolQuery : IRequest<GetSchoolQueryResponse>
    {
        public long? SchoolId { get; set; }
    }
}
