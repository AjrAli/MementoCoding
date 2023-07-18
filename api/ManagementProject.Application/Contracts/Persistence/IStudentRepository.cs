using ManagementProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ManagementProject.Application.Contracts.Persistence
{
    public interface IStudentRepository : IBaseRepository<Student>
    {
        Task<IReadOnlyList<Student>?> GetAllWithIncludeAsync(Expression<Func<Student, object>> navigationPropertyPath);
        Task<Student?> GetByIdWithIncludeAsync(Expression<Func<Student, bool>> predicate, Expression<Func<Student, object>> navigationPropertyPath);
    }
}
