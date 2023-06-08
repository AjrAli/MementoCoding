using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Management.Application.Contracts.Persistence;
using SchoolProject.Management.Application.Exceptions;
using SchoolProject.Management.Application.Features.Response;
using SchoolProject.Management.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolProject.Management.Application.Features.Schools.Queries.GetSchools
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
            var query = _schoolRepository.Queryable;

            if (request.Take != 0)
                query = query.OrderBy(x => x.Name).Skip(request.Skip).Take(request.Take);

            var listSchools = await query.OrderBy(x => x.Name).ToListAsync();

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
