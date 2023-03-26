using MediatR;

namespace SchoolProject.Management.Application.Features.Students.Queries.GetStudent
{
    public class GetStudentQuery : IRequest<GetStudentQueryResponse>
    {
        public long? StudentId { get; set; }
    }
}
