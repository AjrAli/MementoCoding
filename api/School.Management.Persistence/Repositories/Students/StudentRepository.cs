using Microsoft.EntityFrameworkCore;
using SchoolProject.Management.Application.Contracts.Persistence;
using SchoolProject.Management.Domain.Entities;
using SchoolProject.Management.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SchoolProject.Management.Persistence.Repositories.Students
{
    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        public StudentRepository(SchoolManagementDbContext dbContext) : base(dbContext)
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
