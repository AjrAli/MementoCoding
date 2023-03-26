using Microsoft.AspNetCore.Identity;
using SchoolProject.Management.Identity.Entity;
using System.Threading.Tasks;

namespace SchoolProject.Management.Identity.Seed
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
