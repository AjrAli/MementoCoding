using AutoMapper;
using MediatR;
using SchoolProject.Management.Application.Contracts.Persistence;
using SchoolProject.Management.Application.Exceptions;
using SchoolProject.Management.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolProject.Management.Application.Features.Schools.Queries.GetSchool
{
    public class GetSchoolQueryHandler : IRequestHandler<GetSchoolQuery, GetSchoolQueryResponse>
    {
        private readonly IBaseRepository<School> _schoolRepository;
        private readonly IBaseRepository<Student> _studentRepository;
        private readonly IMapper _mapper;

        public GetSchoolQueryHandler(IMapper mapper,
                                      IBaseRepository<School> schoolRepository,
                                      IBaseRepository<Student> studentRepository)
        {
            _mapper = mapper;
            _schoolRepository = schoolRepository;
            _studentRepository = studentRepository;
        }

        public async Task<GetSchoolQueryResponse> Handle(GetSchoolQuery request, CancellationToken cancellationToken)
        {
            var getSchoolQueryResponse = new GetSchoolQueryResponse();
            long Id = (request?.SchoolId != null) ? (long)request!.SchoolId : 0;
            var school = await _schoolRepository.GetAsync(Id);
            if (school == null)
            {
                throw new NotFoundException(nameof(School), Id);
            }
            getSchoolQueryResponse.SchoolDto = _mapper.Map<GetSchoolDto>(school);
            getSchoolQueryResponse.SchoolDto.Haschildren = _studentRepository.Any(x => x.SchoolId == school.Id);
            return getSchoolQueryResponse;
        }
    }
}
