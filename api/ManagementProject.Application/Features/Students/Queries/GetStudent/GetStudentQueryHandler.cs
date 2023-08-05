using AutoMapper;
using MediatR;
using ManagementProject.Application.Contracts.Persistence;
using ManagementProject.Application.Exceptions;
using ManagementProject.Application.Features.Response;
using ManagementProject.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace ManagementProject.Application.Features.Students.Queries.GetStudent
{
    public class GetStudentQueryHandler : IRequestHandler<GetStudentQuery, GetStudentQueryResponse>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public GetStudentQueryHandler(IMapper mapper, IStudentRepository studentRepository)
        {
            _mapper = mapper;
            _studentRepository = studentRepository;
        }

        public async Task<GetStudentQueryResponse> Handle(GetStudentQuery request, CancellationToken cancellationToken)
        {
            long studentId = request?.StudentId ?? 0;

            var student = await _studentRepository.GetByIdWithIncludeAsync(x => x.Id == studentId, navigationPropertyPath: x => x.School);

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
