using DotNetCore.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ManagementProject.Application.Contracts.Persistence;
using ManagementProject.Persistence.Context;
using ManagementProject.Persistence.Repositories;
using ManagementProject.Persistence.Repositories.Schools;
using ManagementProject.Persistence.Repositories.Students;

namespace ManagementProject.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ManagementProjectDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("SchoolManagementDbConnectionString")));

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork<ManagementProjectDbContext>>();

            services.AddScoped<ISchoolRepository, SchoolRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            return services;
        }
    }
}
