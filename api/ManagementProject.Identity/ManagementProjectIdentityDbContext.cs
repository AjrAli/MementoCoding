using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ManagementProject.Identity.Entity;

namespace ManagementProject.Identity
{
    public class ManagementProjectIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public ManagementProjectIdentityDbContext(DbContextOptions<ManagementProjectIdentityDbContext> options) : base(options)
        {

        }
    }
}
