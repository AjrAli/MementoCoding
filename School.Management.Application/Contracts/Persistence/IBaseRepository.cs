using DotNetCore.Repositories;

namespace SchoolProject.Management.Application.Contracts.Persistence
{
    public interface IBaseRepository<T> : IRepository<T> where T : class
    {
    }
}
