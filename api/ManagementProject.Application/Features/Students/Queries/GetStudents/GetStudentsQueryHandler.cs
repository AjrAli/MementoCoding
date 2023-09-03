using AutoMapper;
using ManagementProject.Application.Exceptions;
using ManagementProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ManagementProject.Persistence.Context;

namespace ManagementProject.Application.Features.Students.Queries.GetStudents
{
    public class GetStudentsQueryHandler : IRequestHandler<GetStudentsQuery, GetStudentsQueryResponse>
    {
        private readonly ManagementProjectDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetStudentsQueryHandler(IMapper mapper, ManagementProjectDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<GetStudentsQueryResponse> Handle(GetStudentsQuery request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Students.AsQueryable();
            var count = await _dbContext.Students.CountAsync(cancellationToken);
            if (request.Options != null)
            {
                query = (IQueryable<Student>)request.Options.ApplyTo(query);

                if (request.Options.Filter != null)
                {
                    var filterExpression = request.Options.Filter;
                    var filteredQuery = (IQueryable<Student>)filterExpression.ApplyTo(_dbContext.Students.AsQueryable(), new ODataQuerySettings());
                    count = await filteredQuery.CountAsync(cancellationToken);
                    return await GetStudentsResponse(query, count, cancellationToken);
                }
            }

            return await GetStudentsResponse(query, count, cancellationToken);
        }

        private async Task<GetStudentsQueryResponse> GetStudentsResponse(IQueryable<Student> query, long count, CancellationToken cancellationToken)
        {
            var listStudents = await (query.Count() > 0 ? query.Include(x => x.School).ToListAsync(cancellationToken) : Task.FromResult<List<Student>>(null)) ?? throw new NotFoundException($"No students found");
            return new GetStudentsQueryResponse
            {
                Count = count,
                StudentsDto = _mapper.Map<List<GetStudentsDto>>(listStudents)
            };
        }
    }
}
