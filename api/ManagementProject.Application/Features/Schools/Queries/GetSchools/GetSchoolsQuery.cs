using ManagementProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.OData.Query;

namespace ManagementProject.Application.Features.Schools.Queries.GetSchools
{
    public class GetSchoolsQuery : IRequest<GetSchoolsQueryResponse>
    {
        public ODataQueryOptions<School>? Options { get;set; }
    }
}
