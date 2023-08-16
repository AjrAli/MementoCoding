using AutoMapper;
using ManagementProject.Application.Contracts.Persistence;
using ManagementProject.Application.Exceptions;
using ManagementProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ManagementProject.Application.Features.Students.Queries.GetStudents
{
    public class GetStudentsQueryHandler : IRequestHandler<GetStudentsQuery, GetStudentsQueryResponse>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public GetStudentsQueryHandler(IMapper mapper, IStudentRepository studentRepository)
        {
            _mapper = mapper;
            _studentRepository = studentRepository;
        }

        public async Task<GetStudentsQueryResponse> Handle(GetStudentsQuery request, CancellationToken cancellationToken)
        {
            var query = _studentRepository.GetDbSetQueryable();
            var count = await _studentRepository.CountAsync();
            if (request.Options != null)
            {
                query = (IQueryable<Student>)request.Options.ApplyTo(query);

                if (request.Options.Filter != null)
                {
                    var filterExpression = request.Options.Filter;
                    var filteredQuery = (IQueryable<Student>)filterExpression.ApplyTo(_studentRepository.GetDbSetQueryable(), new ODataQuerySettings());
                    count = await filteredQuery.CountAsync(CancellationToken.None);
                    return await GetStudentsResponse(query, count);
                }
            }

            return await GetStudentsResponse(query, count);
        }

        private async Task<GetStudentsQueryResponse> GetStudentsResponse(IQueryable<Student> query, long count)
        {
            var listStudents = await (query != null ? query.Include(x => x.School).ToListAsync() : Task.FromResult<List<Student>>(null)) ?? throw new NotFoundException($"No students found");
            return new GetStudentsQueryResponse
            {
                Count = count,
                StudentsDto = _mapper.Map<List<GetStudentsDto>>(listStudents)
            };
        }
    }
}
