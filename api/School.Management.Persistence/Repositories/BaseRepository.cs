using DotNetCore.EntityFrameworkCore;
using DotNetCore.Repositories;
using SchoolProject.Management.Application.Contracts.Persistence;
using SchoolProject.Management.Domain.Entities;
using SchoolProject.Management.Persistence.Context;
using System.Linq;

namespace SchoolProject.Management.Persistence.Repositories
{
    public class BaseRepository<T> : Repository<T>, IBaseRepository<T> where T : class
    {
        protected readonly SchoolManagementDbContext _dbContext;

        public BaseRepository(SchoolManagementDbContext dbContext) : base(new EFCommandRepository<T>(dbContext), new EFQueryRepository<T>(dbContext))
        {
            _dbContext = dbContext;
        }
        public IQueryable<T> GetDbSetQueryable()
        {
            return _dbContext.Set<T>().AsQueryable();
        }
    }
}
