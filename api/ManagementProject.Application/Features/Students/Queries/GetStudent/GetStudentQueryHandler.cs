using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ManagementProject.Application.Contracts.MediatR.Query;
using ManagementProject.Application.Exceptions;
using ManagementProject.Domain.Entities;
using ManagementProject.Persistence.Context;

namespace ManagementProject.Application.Features.Students.Queries.GetStudent
{
    public class GetStudentQueryHandler : IQueryHandler<GetStudentQuery, GetStudentQueryResponse>
    {
        private readonly ManagementProjectDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetStudentQueryHandler(IMapper mapper, ManagementProjectDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<GetStudentQueryResponse> Handle(GetStudentQuery request, CancellationToken cancellationToken)
        {
            long studentId = request?.StudentId ?? 0;

            var student = await _dbContext.GetStudentByIdIncludeSchoolAsync(studentId);

            if (student == null)
            {
                throw new NotFoundException(nameof(Student), studentId);
            }

            return new GetStudentQueryResponse
            {
                StudentDto = _mapper.Map<GetStudentDto>(student)
            };
        }
    }
}
