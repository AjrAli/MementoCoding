using MediatR;

namespace ManagementProject.Application.Features.Students.Queries.GetStudent
{
    public class GetStudentQuery : IRequest<GetStudentQueryResponse>
    {
        public long? StudentId { get; set; }
    }
}
