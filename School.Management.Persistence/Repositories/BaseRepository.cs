using SchoolProject.Management.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SchoolProject.Management.Persistence.Context;
using System.Linq.Expressions;
using DotNetCore.EntityFrameworkCore;
using DotNetCore.Repositories;

namespace SchoolProject.Management.Persistence.Repositories
{
    public class BaseRepository<T> : Repository<T>, IBaseRepository<T> where T : class
    {
        protected readonly SchoolManagementDbContext _dbContext;

        public BaseRepository(SchoolManagementDbContext dbContext) : base(new EFCommandRepository<T>(dbContext), new EFQueryRepository<T>(dbContext))
        {
            _dbContext = dbContext;
        }
    }
}
