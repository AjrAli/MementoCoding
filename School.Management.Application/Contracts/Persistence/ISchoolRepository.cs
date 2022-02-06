using SchoolProject.Management.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolProject.Management.Application.Contracts.Persistence
{
    public interface ISchoolRepository : IBaseRepository<School>
    {
        Task<List<School>> GetSchoolsWithStudents();
    }
}
