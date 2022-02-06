using DotNetCore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Management.Application.Contracts.Persistence
{
    public interface IBaseRepository<T> : IRepository<T>, ICommandRepository<T>, IQueryRepository<T> where T : class
    {
    }
}
