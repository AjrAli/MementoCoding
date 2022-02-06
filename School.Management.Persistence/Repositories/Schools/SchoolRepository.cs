using Microsoft.EntityFrameworkCore;
using SchoolProject.Management.Application.Contracts.Persistence;
using SchoolProject.Management.Domain.Entities;
using SchoolProject.Management.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolProject.Management.Persistence.Repositories.Schools
{
    public class SchoolRepository : BaseRepository<School>, ISchoolRepository
    {
        public SchoolRepository(SchoolManagementDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<School>> GetSchoolsWithStudents()
        {
            var allSchools = await _dbContext.Schools.Include(x => x.Students).ToListAsync();
            return allSchools;
        }
    }
}
