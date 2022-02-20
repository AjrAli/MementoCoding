using AutoMapper;
using MediatR;
using SchoolProject.Management.Application.Contracts.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolProject.Management.Application.Features.Students.Queries.GetStudents
{
    public class GetStudentsQueryHandler : IRequestHandler<GetStudentsQuery, GetStudentsQueryResponse>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public GetStudentsQueryHandler(IMapper mapper,
                                      IStudentRepository studentRepository)
        {
            _mapper = mapper;
            _studentRepository = studentRepository;
        }

        public async Task<GetStudentsQueryResponse> Handle(GetStudentsQuery request, CancellationToken cancellationToken)
        {
            var getStudentsQueryResponse = new GetStudentsQueryResponse();
            var allStudents = (await _studentRepository.GetAllWithIncludeAsync(x => x.School)).OrderBy(x => x.LastName).ToList();
            getStudentsQueryResponse.StudentsDto = _mapper.Map<List<GetStudentsDto>>(allStudents);
            return getStudentsQueryResponse;
        }
    }
}
