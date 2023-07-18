using DotNetCore.EntityFrameworkCore;
using DotNetCore.Repositories;
using ManagementProject.Application.Contracts.Persistence;
using ManagementProject.Domain.Entities;
using ManagementProject.Persistence.Context;
using System.Linq;

namespace ManagementProject.Persistence.Repositories
{
    public class BaseRepository<T> : Repository<T>, IBaseRepository<T> where T : class
    {
        protected readonly ManagementProjectDbContext _dbContext;

        public BaseRepository(ManagementProjectDbContext dbContext) : base(new EFCommandRepository<T>(dbContext), new EFQueryRepository<T>(dbContext))
        {
            _dbContext = dbContext;
        }
        public IQueryable<T> GetDbSetQueryable()
        {
            return _dbContext.Set<T>().AsQueryable();
        }
    }
}
