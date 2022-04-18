using Autofac;
using SchoolProject.Management.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Management.Identity
{
    public class IdentityModule  : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyApp = Assembly.GetAssembly(typeof(ApplicationModule));
            var assemblyApi = Assembly.GetEntryAssembly();
            if (assemblyApp != null && assemblyApi != null)
                builder.RegisterAssemblyTypes(assembly, assemblyApp, assemblyApi)
                       .Where(t => t.Name.EndsWith("Service"))
                       .AsImplementedInterfaces()
                       .InstancePerLifetimeScope();
        }
    }
}
