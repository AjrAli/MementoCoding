using System.Threading.Tasks;

namespace SchoolProject.Management.Persistence.Context
{
    public class SchoolManagementUnitOfWork : ISchoolManagementUnitOfWork
    {
        public SchoolManagementUnitOfWork(SchoolManagementDbContext context)
        {
            Context = context;
        }

        private SchoolManagementDbContext Context { get; }

        public int SaveChanges()
        {
            return Context.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return Context.SaveChangesAsync();
        }
    }
}
