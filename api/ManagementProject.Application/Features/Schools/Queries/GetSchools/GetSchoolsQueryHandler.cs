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
using Microsoft.AspNetCore.OData.Query;

namespace ManagementProject.Application.Features.Schools.Queries.GetSchools
{
    public class GetSchoolsQueryHandler : IRequestHandler<GetSchoolsQuery, GetSchoolsQueryResponse>
    {
        private readonly IBaseRepository<School> _schoolRepository;
        private readonly IBaseRepository<Student> _studentRepository;
        private readonly IMapper _mapper;

        public GetSchoolsQueryHandler(IMapper mapper, IBaseRepository<School> schoolRepository, IBaseRepository<Student> studentRepository)
        {
            _mapper = mapper;
            _schoolRepository = schoolRepository;
            _studentRepository = studentRepository;
        }

        public async Task<GetSchoolsQueryResponse> Handle(GetSchoolsQuery request, CancellationToken cancellationToken)
        {
            var query = _schoolRepository.GetDbSetQueryable();
            var count = await _schoolRepository.CountAsync();
            if (request.Options != null)
            {
                query = (IQueryable<School>)request.Options.ApplyTo(query);

                if (request.Options.Filter != null)
                {
                    var filterExpression = request.Options.Filter;
                    var filteredQuery = (IQueryable<School>)filterExpression.ApplyTo(_schoolRepository.GetDbSetQueryable(), new ODataQuerySettings());
                    count = await filteredQuery.CountAsync();
                }
            }

            var schools = await (query != null ? query.ToListAsync() : Task.FromResult<List<School>>(null));
            if (schools == null)
            {
                throw new NotFoundException($"No schools found");
            }
            var schoolsWithStudents = schools
                .Select(school => new
                {
                    School = school,
                    Students = _studentRepository.Queryable
                                .Any(student => student.SchoolId == school.Id)
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
