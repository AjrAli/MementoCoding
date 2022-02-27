﻿using AutoMapper;
using MediatR;
using SchoolProject.Management.Application.Contracts.Persistence;
using SchoolProject.Management.Application.Exceptions;
using SchoolProject.Management.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace SchoolProject.Management.Application.Features.Students.Queries.GetStudent
{
    public class GetStudentQueryHandler : IRequestHandler<GetStudentQuery, GetStudentQueryResponse>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public GetStudentQueryHandler(IMapper mapper,
                                      IStudentRepository studentRepository)
        {
            _mapper = mapper;
            _studentRepository = studentRepository;
        }

        public async Task<GetStudentQueryResponse> Handle(GetStudentQuery request, CancellationToken cancellationToken)
        {
            var getStudentQueryResponse = new GetStudentQueryResponse();
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
