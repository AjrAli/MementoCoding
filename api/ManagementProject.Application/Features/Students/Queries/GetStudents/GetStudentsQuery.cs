using ManagementProject.Application.Contracts.MediatR.Query;
using ManagementProject.Domain.Entities;
using Microsoft.AspNetCore.OData.Query;

namespace ManagementProject.Application.Features.Students.Queries.GetStudents
{
    public class GetStudentsQuery : IQuery<GetStudentsQueryResponse>
    {
        public ODataQueryOptions<Student>? Options { get; set; }
    }
}
