using Microsoft.AspNetCore.Identity;


namespace ManagementProject.Identity.Entity
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
