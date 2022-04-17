using Autofac;
using DotNetCore.EntityFrameworkCore;
using SchoolProject.Management.Application.Contracts.Persistence;
using SchoolProject.Management.Persistence.Context;
using SchoolProject.Management.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Management.Persistence
{
    public class PersistencetModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var dataAccess = Assembly.GetExecutingAssembly();

            builder.RegisterType<UnitOfWork<SchoolManagementDbContext>>()
                   .As<IUnitOfWork>()
                   .InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(dataAccess)
                   .Where(t => t.Name.EndsWith("Repository"))
                   .AsImplementedInterfaces();
        }
    }
}
