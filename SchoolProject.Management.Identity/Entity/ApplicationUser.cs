using Microsoft.AspNetCore.Identity;


namespace SchoolProject.Management.Identity.Entity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
