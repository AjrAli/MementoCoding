using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ManagementProject.Persistence.Context;

namespace ManagementProject.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ManagementProjectDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("ManagementProjectDbConnectionString")));
            return services;
        }
    }
}
