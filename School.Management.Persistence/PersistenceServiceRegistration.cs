using SchoolProject.Management.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SchoolProject.Management.Persistence.Context;
using SchoolProject.Management.Persistence.Repositories;
using SchoolProject.Management.Persistence.Repositories.Students;
using SchoolProject.Management.Persistence.Repositories.Schools;
using Microsoft.Extensions.Logging;
using DotNetCore.EntityFrameworkCore;

namespace SchoolProject.Management.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SchoolManagementDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("SchoolManagementDbConnectionString")));

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork<SchoolManagementDbContext>>();

            services.AddScoped<ISchoolRepository, SchoolRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            return services;
        }
    }
}
