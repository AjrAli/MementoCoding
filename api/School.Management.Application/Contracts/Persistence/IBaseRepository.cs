using DotNetCore.Repositories;
using System.Linq;

namespace SchoolProject.Management.Application.Contracts.Persistence
{
    public interface IBaseRepository<T> : IRepository<T> where T : class
    {
        IQueryable<T> GetDbSetQueryable();
    }
}
