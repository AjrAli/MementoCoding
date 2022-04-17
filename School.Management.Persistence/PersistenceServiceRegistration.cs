using DotNetCore.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SchoolProject.Management.Application.Contracts.Persistence;
using SchoolProject.Management.Persistence.Context;
using SchoolProject.Management.Persistence.Repositories;
using SchoolProject.Management.Persistence.Repositories.Schools;
using SchoolProject.Management.Persistence.Repositories.Students;

namespace SchoolProject.Management.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SchoolManagementDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("SchoolManagementDbConnectionString")));
            return services;
        }
    }
}
