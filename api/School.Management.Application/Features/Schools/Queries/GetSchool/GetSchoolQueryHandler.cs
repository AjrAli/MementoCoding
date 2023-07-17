using AutoMapper;
using MediatR;
using SchoolProject.Management.Application.Contracts.Persistence;
using SchoolProject.Management.Application.Exceptions;
using SchoolProject.Management.Application.Features.Response;
using SchoolProject.Management.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using SchoolProject.Management.Application.Features.Students.Queries.GetStudents;
using System.Collections.Generic;

namespace SchoolProject.Management.Application.Features.Schools.Queries.GetSchool
{
    public class GetSchoolQueryHandler : IRequestHandler<GetSchoolQuery, GetSchoolQueryResponse>
    {
        private readonly ISchoolRepository _schoolRepository;
        private readonly IMapper _mapper;
        private readonly IResponseFactory<GetSchoolQueryResponse> _responseFactory;
        public GetSchoolQueryHandler(IMapper mapper,
                                      ISchoolRepository schoolRepository,
                                      IResponseFactory<GetSchoolQueryResponse> responseFactory)
        {
            _mapper = mapper;
            _schoolRepository = schoolRepository;
            _responseFactory = responseFactory;
        }

        public async Task<GetSchoolQueryResponse> Handle(GetSchoolQuery request, CancellationToken cancellationToken)
        {
            var getSchoolQueryResponse = _responseFactory.CreateResponse();
            long Id = (request?.SchoolId != null) ? (long)request!.SchoolId : 0;
            var school = await _schoolRepository.GetSchoolWithStudents(Id);
            if (school == null)
            {
                throw new NotFoundException(nameof(School), Id);
            }
            getSchoolQueryResponse.SchoolDto = _mapper.Map<GetSchoolDto>(school);
            getSchoolQueryResponse.SchoolDto.Students = _mapper.Map<List<GetStudentsDto>>(school.Students);
            getSchoolQueryResponse.SchoolDto.Haschildren = school.Students.Any(x => x.SchoolId == school.Id);
            return getSchoolQueryResponse;
        }
    }
}
