using AutoMapper;
using MediatR;
using ManagementProject.Application.Contracts.Persistence;
using ManagementProject.Application.Exceptions;
using ManagementProject.Application.Features.Response;
using ManagementProject.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace ManagementProject.Application.Features.Schools.Queries.GetSchool
{
    public class GetSchoolQueryHandler : IRequestHandler<GetSchoolQuery, GetSchoolQueryResponse>
    {
        private readonly ISchoolRepository _schoolRepository;
        private readonly IMapper _mapper;

        public GetSchoolQueryHandler(IMapper mapper, ISchoolRepository schoolRepository)
        {
            _mapper = mapper;
            _schoolRepository = schoolRepository;
        }

        public async Task<GetSchoolQueryResponse> Handle(GetSchoolQuery request, CancellationToken cancellationToken)
        {
            var school = await _schoolRepository.GetAsync(request?.SchoolId ?? 0);
            if (school == null)
            {
                throw new NotFoundException(nameof(School), request?.SchoolId ?? 0);
            }

            var schoolDto = _mapper.Map<GetSchoolDto>(school);
            schoolDto.Haschildren = school.Students.Any(x => x.SchoolId == school.Id);

            return new GetSchoolQueryResponse
            {
                SchoolDto = schoolDto
            };
        }
    }
}
