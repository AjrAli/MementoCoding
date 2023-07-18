using Microsoft.EntityFrameworkCore;
using ManagementProject.Application.Contracts.Persistence;
using ManagementProject.Domain.Entities;
using ManagementProject.Persistence.Context;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable disable
namespace ManagementProject.Persistence.Repositories.Schools
{
    public class SchoolRepository : BaseRepository<School>, ISchoolRepository
    {
        public SchoolRepository(ManagementProjectDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<School>> GetSchoolsWithStudents()
        {
            var allSchools = await _dbContext.Schools?.Include(x => x.Students).ToListAsync();
            return allSchools;
        }

        public async Task<School> GetSchoolWithStudents(long schoolId)
        {
            return await _dbContext.Schools.Include(x => x.Students)?.FirstOrDefaultAsync(x => x.Id == schoolId);
        }
    }
}
