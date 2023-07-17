using SchoolProject.Management.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolProject.Management.Application.Contracts.Persistence
{
    public interface ISchoolRepository : IBaseRepository<School>
    {
        Task<School> GetSchoolWithStudents(long schoolId);
        Task<List<School>> GetSchoolsWithStudents();
    }
}
