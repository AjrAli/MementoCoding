﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
            var query = _studentRepository.Queryable;

            if (request.Take != 0)
                query = query.OrderBy(x => x.LastName).Skip(request.Skip).Take(request.Take);

            var listStudents = await query.Include(x => x.School).OrderBy(x => x.LastName).ToListAsync();

            if (listStudents == null)
            {
                throw new NotFoundException($"No students found");
            }
            getStudentsQueryResponse.StudentsDto = _mapper.Map<List<GetStudentsDto>>(listStudents);
            getStudentsQueryResponse.Count = (await _studentRepository.CountAsync());
            return getStudentsQueryResponse;
        }
    }
}
