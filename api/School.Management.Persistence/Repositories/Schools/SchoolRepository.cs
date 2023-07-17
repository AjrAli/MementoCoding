using Microsoft.EntityFrameworkCore;
using SchoolProject.Management.Application.Contracts.Persistence;
using SchoolProject.Management.Domain.Entities;
using SchoolProject.Management.Persistence.Context;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable disable
namespace SchoolProject.Management.Persistence.Repositories.Schools
{
    public class SchoolRepository : BaseRepository<School>, ISchoolRepository
    {
        public SchoolRepository(SchoolManagementDbContext dbContext) : base(dbContext)
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
