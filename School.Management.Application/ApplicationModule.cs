using Autofac;
using MediatR;
using SchoolProject.Management.Application.Features.Response;
using System.Linq;
using System.Reflection;
namespace SchoolProject.Management.Application
{
    public class ApplicationModule : Autofac.Module
    {

        protected override void Load(ContainerBuilder builder)
        {
            var assembly = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyOpenGenericTypes(assembly)
                    .Where(t => t.GetTypeInfo().ImplementedInterfaces.Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IPipelineBehavior<,>)))
                    .AsImplementedInterfaces()
                    .InstancePerLifetimeScope();
            builder.RegisterAssemblyOpenGenericTypes(assembly)
                    .Where(t => t.GetTypeInfo().ImplementedInterfaces.Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IResponseFactory<>)))
                    .AsImplementedInterfaces()
                    .InstancePerLifetimeScope();
        }
    }
}
