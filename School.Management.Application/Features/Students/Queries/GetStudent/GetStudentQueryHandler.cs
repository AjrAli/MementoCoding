using AutoMapper;
using MediatR;
using SchoolProject.Management.Application.Contracts.Persistence;
using SchoolProject.Management.Application.Exceptions;
using SchoolProject.Management.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
            long Id = (long) request.StudentId;
            var student = await _studentRepository.GetByIdWithIncludeAsync(x => x.Id == Id, x => x.School);
            if (student == null)
            {
                throw new NotFoundException(nameof(Student), request.StudentId);
            }
            getStudentQueryResponse.StudentDto = _mapper.Map<GetStudentDto>(student);
            return getStudentQueryResponse;
        }
    }
}
