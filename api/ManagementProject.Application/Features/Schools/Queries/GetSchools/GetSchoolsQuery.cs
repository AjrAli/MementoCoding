using ManagementProject.Application.Contracts.MediatR.Query;
using ManagementProject.Domain.Entities;
using Microsoft.AspNetCore.OData.Query;

namespace ManagementProject.Application.Features.Schools.Queries.GetSchools
{
    public class GetSchoolsQuery : IQuery<GetSchoolsQueryResponse>
    {
        public ODataQueryOptions<School>? Options { get;set; }
    }
}
