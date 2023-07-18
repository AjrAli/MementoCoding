using MediatR;

namespace ManagementProject.Application.Features.Students.Queries.GetStudents
{
    public class GetStudentsQuery : IRequest<GetStudentsQueryResponse>
    {
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}
