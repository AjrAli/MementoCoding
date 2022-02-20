using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SchoolProject.Management.Application.Features.Service;
using System.Reflection;

namespace SchoolProject.Management.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddScoped<IResponseHandlingService, ResponseHandlingService>();
            return services;
        }
    }
}
