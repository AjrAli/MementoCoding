using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ManagementProject.Application.Contracts.Persistence;
using ManagementProject.Application.Exceptions;
using ManagementProject.Application.Features.Response;
using ManagementProject.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ManagementProject.Application.Features.Schools.Queries.GetSchools
{
    public class GetSchoolsQueryHandler : IRequestHandler<GetSchoolsQuery, GetSchoolsQueryResponse>
    {
        private readonly IBaseRepository<School> _schoolRepository;
        private readonly IBaseRepository<Student> _studentRepository;
        private readonly IMapper _mapper;
        private readonly IResponseFactory<GetSchoolsQueryResponse> _responseFactory;
        public GetSchoolsQueryHandler(IMapper mapper,
                                      IBaseRepository<School> schoolRepository,
                                      IBaseRepository<Student> studentRepository,
                                      IResponseFactory<GetSchoolsQueryResponse> responseFactory)
        {
            _mapper = mapper;
            _schoolRepository = schoolRepository;
            _studentRepository = studentRepository;
            _responseFactory = responseFactory;
        }

        public async Task<GetSchoolsQueryResponse> Handle(GetSchoolsQuery request, CancellationToken cancellationToken)
        {
            var getSchoolsQueryResponse = _responseFactory.CreateResponse();
            int index = 0;
            var query = _schoolRepository.GetDbSetQueryable();

            if (request.Take != 0)
                query = query.OrderBy(x => x.Name).Skip(request.Skip).Take(request.Take);

            var listSchools = query != null ? await query.OrderBy(x => x.Name).ToListAsync() : null;

            if (listSchools == null)
            {
                throw new NotFoundException($"No schools found");
            }
            getSchoolsQueryResponse.SchoolsDto = _mapper.Map<List<GetSchoolsDto>>(listSchools);
            getSchoolsQueryResponse.Count = (await _schoolRepository.CountAsync());
            foreach (var school in listSchools)
            {
                getSchoolsQueryResponse.SchoolsDto[index].Haschildren = (await _studentRepository.ListAsync()).Any(x => x.SchoolId == school.Id);
                index++;
            }


            return getSchoolsQueryResponse;
        }
    }
}
