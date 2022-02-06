using System.Threading.Tasks;

namespace SchoolProject.Management.Persistence.Context
{
    public interface ISchoolManagementUnitOfWork
    {
        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}
