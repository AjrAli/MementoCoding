using Microsoft.AspNetCore.Identity;
using ManagementProject.Identity.Entity;
using System.Threading.Tasks;
using ManagementProject.Identity.Roles;

namespace ManagementProject.Identity.Seed
{
    public static class CreateFirstUser
    {
        public async static Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            var newUser = new ApplicationUser()
            {
                FirstName = "Ali",
                LastName = "Admin",
                Email = "ali@gmail.com",
                UserName = "admin",
                EmailConfirmed = true //For test app only
            };
            var user = await userManager.FindByNameAsync(newUser.UserName);
            if (user == null)
            {
                await userManager.CreateAsync(newUser, "admin");
                await userManager.AddToRoleAsync(newUser, RoleNames.Administrator);
            }
            if (user != null && (await userManager.GetRolesAsync(user)).Count == 0)
            {
                await userManager.AddToRoleAsync(user, RoleNames.Administrator);
            }
        }
    }
}
