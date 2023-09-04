using ManagementProject.Application.Contracts.MediatR.Query;

namespace ManagementProject.Application.Features.Students.Queries.GetStudent
{
    public class GetStudentQuery : IQuery<GetStudentQueryResponse>
    {
        public long? StudentId { get; set; }
    }
}
