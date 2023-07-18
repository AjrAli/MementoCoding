using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ManagementProject.Application.Features.PipelineBehaviours;
using ManagementProject.Application.Features.Response;
using ManagementProject.Application.Features.Service;
using System.Reflection;

namespace ManagementProject.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
            services.AddTransient(typeof(IResponseFactory<>), typeof(ResponseFactory<>));
            return services;
        }
    }
}
