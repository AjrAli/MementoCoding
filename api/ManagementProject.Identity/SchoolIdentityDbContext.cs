using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ManagementProject.Identity.Entity;

namespace ManagementProject.Identity
{
    public class SchoolIdentityDbContext: IdentityDbContext<ApplicationUser>
    {
        public SchoolIdentityDbContext(DbContextOptions<SchoolIdentityDbContext> options) : base(options)
        {

        }
    }
}
