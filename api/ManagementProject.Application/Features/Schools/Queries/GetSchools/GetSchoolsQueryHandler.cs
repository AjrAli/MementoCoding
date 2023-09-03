using MediatR;
using Microsoft.EntityFrameworkCore;
using ManagementProject.Application.Exceptions;
using ManagementProject.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ManagementProject.Persistence.Context;
using Microsoft.AspNetCore.OData.Query;

namespace ManagementProject.Application.Features.Schools.Queries.GetSchools
{
    public class GetSchoolsQueryHandler : IRequestHandler<GetSchoolsQuery, GetSchoolsQueryResponse>
    {
        private readonly ManagementProjectDbContext _dbContext;

        public GetSchoolsQueryHandler(ManagementProjectDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetSchoolsQueryResponse> Handle(GetSchoolsQuery request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Schools.AsQueryable();
            var count = await _dbContext.Schools.CountAsync(cancellationToken);
            if (request.Options != null)
            {
                query = (IQueryable<School>)request.Options.ApplyTo(query);

                if (request.Options.Filter != null)
                {
                    var filterExpression = request.Options.Filter;
                    var filteredQuery = (IQueryable<School>)filterExpression.ApplyTo(_dbContext.Schools.AsQueryable(), new ODataQuerySettings());
                    count = await filteredQuery.CountAsync(cancellationToken);
                }
            }

            var schools = await (await query.CountAsync() != 0 ? query.ToListAsync(cancellationToken) : Task.FromResult<List<School>>(null)) ?? throw new NotFoundException($"No schools found");
            var schoolsWithStudents = schools
                .Select(school => new
                {
                    School = school,
                    Students = _dbContext.Students.Any(student => student.SchoolId == school.Id)
                })
                .ToList();

            var schoolsDto = schoolsWithStudents.Select(school => new GetSchoolsDto
            {
                Id = school.School.Id,
                Name = school.School.Name,
                Adress = school.School.Adress,
                Town = school.School.Town,
                Description = school.School.Description,
                Haschildren = school.Students
            }).ToList();

            return new GetSchoolsQueryResponse
            {
                SchoolsDto = schoolsDto,
                Count = count
            };
        }
    }
}
