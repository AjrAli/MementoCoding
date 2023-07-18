using ManagementProject.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManagementProject.Application.Contracts.Persistence
{
    public interface ISchoolRepository : IBaseRepository<School>
    {
        Task<School> GetSchoolWithStudents(long schoolId);
        Task<List<School>> GetSchoolsWithStudents();
    }
}
