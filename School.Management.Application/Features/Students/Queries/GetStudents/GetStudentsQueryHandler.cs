using AutoMapper;
using MediatR;
using SchoolProject.Management.Application.Contracts.Persistence;
using SchoolProject.Management.Application.Exceptions;
using SchoolProject.Management.Application.Features.Response;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


#nullable disable
namespace SchoolProject.Management.Application.Features.Students.Queries.GetStudents
{
    public class GetStudentsQueryHandler : IRequestHandler<GetStudentsQuery, GetStudentsQueryResponse>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;
        private readonly IResponseFactory<GetStudentsQueryResponse> _responseFactory;

        public GetStudentsQueryHandler(IMapper mapper,
                                      IStudentRepository studentRepository,
                                      IResponseFactory<GetStudentsQueryResponse> responseFactory)
        {
            _mapper = mapper;
            _studentRepository = studentRepository;
            _responseFactory = responseFactory;
        }

        public async Task<GetStudentsQueryResponse> Handle(GetStudentsQuery request, CancellationToken cancellationToken)
        {
            var getStudentsQueryResponse = _responseFactory.CreateResponse();
            var allStudents = (await _studentRepository.GetAllWithIncludeAsync(navigationPropertyPath: x => x.School))?.OrderBy(x => x.LastName)?.ToList();
            if (allStudents == null)
            {
                throw new NotFoundException($"No students found");
            }
            getStudentsQueryResponse.StudentsDto = _mapper.Map<List<GetStudentsDto>>(allStudents);
            return getStudentsQueryResponse;
        }
    }
}
