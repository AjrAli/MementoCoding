using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Management.Identity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Management.Identity
{
    public class SchoolIdentityDbContext: IdentityDbContext<ApplicationUser>
    {
        public SchoolIdentityDbContext(DbContextOptions<SchoolIdentityDbContext> options) : base(options)
        {

        }
    }
}
