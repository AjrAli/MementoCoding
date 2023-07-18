using AutoMapper;
using MediatR;
using ManagementProject.Application.Contracts.Persistence;
using ManagementProject.Application.Exceptions;
using ManagementProject.Application.Features.Response;
using ManagementProject.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace ManagementProject.Application.Features.Students.Queries.GetStudent
{
    public class GetStudentQueryHandler : IRequestHandler<GetStudentQuery, GetStudentQueryResponse>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;
        private readonly IResponseFactory<GetStudentQueryResponse> _responseFactory;

        public GetStudentQueryHandler(IMapper mapper,
                                      IStudentRepository studentRepository,
                                      IResponseFactory<GetStudentQueryResponse> responseFactory)
        {
            _mapper = mapper;
            _studentRepository = studentRepository;
            _responseFactory = responseFactory;
        }

        public async Task<GetStudentQueryResponse> Handle(GetStudentQuery request, CancellationToken cancellationToken)
        {
            var getStudentQueryResponse = _responseFactory.CreateResponse();
            long Id = (request?.StudentId != null) ? (long)request!.StudentId : 0;
            var student = await _studentRepository.GetByIdWithIncludeAsync(x => x.Id == Id, navigationPropertyPath: x => x.School);
            if (student == null)
            {
                throw new NotFoundException(nameof(Student), Id);
            }
            getStudentQueryResponse.StudentDto = _mapper.Map<GetStudentDto>(student);
            return getStudentQueryResponse;
        }
    }
}
