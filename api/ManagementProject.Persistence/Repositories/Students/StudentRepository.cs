using Microsoft.EntityFrameworkCore;
using ManagementProject.Application.Contracts.Persistence;
using ManagementProject.Domain.Entities;
using ManagementProject.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ManagementProject.Persistence.Repositories.Students
{
    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        public StudentRepository(ManagementProjectDbContext dbContext) : base(dbContext)
        {
        }
        public async Task<IReadOnlyList<Student>?> GetAllWithIncludeAsync(Expression<Func<Student, object>> navigationPropertyPath)
        {
            return await _dbContext.Set<Student>().Include(navigationPropertyPath).ToListAsync();
        }

        public async Task<Student?> GetByIdWithIncludeAsync(Expression<Func<Student, bool>> predicate, Expression<Func<Student, object>> navigationPropertyPath)
        {
            return await _dbContext.Set<Student>().Include(navigationPropertyPath).FirstOrDefaultAsync(predicate);

        }
    }
}
