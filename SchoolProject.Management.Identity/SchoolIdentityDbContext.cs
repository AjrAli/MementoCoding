using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Management.Identity.Entity;

namespace SchoolProject.Management.Identity
{
    public class SchoolIdentityDbContext: IdentityDbContext<ApplicationUser>
    {
        public SchoolIdentityDbContext(DbContextOptions<SchoolIdentityDbContext> options) : base(options)
        {

        }
    }
}
