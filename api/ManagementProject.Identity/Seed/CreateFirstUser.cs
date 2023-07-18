using Microsoft.AspNetCore.Identity;
using ManagementProject.Identity.Entity;
using System.Threading.Tasks;

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
                EmailConfirmed = true
            };

            if((await userManager.FindByNameAsync(newUser.UserName)) == null)
            {
                await userManager.CreateAsync(newUser, "admin");
            }
        }

    }
}
