using ManagementProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.OData.Query;

namespace ManagementProject.Application.Features.Students.Queries.GetStudents
{
    public class GetStudentsQuery : IRequest<GetStudentsQueryResponse>
    {
        public ODataQueryOptions<Student>? Options { get; set; }
    }
}
