﻿using AutoMapper;
using MediatR;
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
            var allSchools = (await _schoolRepository.ListAsync())?.OrderBy(x => x.Name)?.ToList();
            if (allSchools == null)
            {
                throw new NotFoundException($"No schools found");
            }
            getSchoolsQueryResponse.SchoolsDto = _mapper.Map<List<GetSchoolsDto>>(allSchools);
            foreach (var school in allSchools)
            {
                getSchoolsQueryResponse.SchoolsDto[index].Haschildren = (await _studentRepository.ListAsync()).Any(x => x.SchoolId == school.Id);
                index++;
            }


            return getSchoolsQueryResponse;
        }
    }
}
