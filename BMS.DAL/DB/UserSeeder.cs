using Microsoft.AspNetCore.Identity;
using BMS.DAL.Constants;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace BMS.DAL.DB;

public class UserSeeder
{


    public static async Task SeedUsersAsync(IServiceProvider serviceProvider)
     {

        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

        //seed ADMIN ACCOUNT 
        await CreateUserWithRole(userManager, "admin@dwvops1.com", "Admin123!", Roles.Admin);
  

    }

    public static async Task CreateUserWithRole
        (UserManager<IdentityUser> userManager, 
        string email , 
        string password , 
        string role)
    {
        if (await userManager.FindByEmailAsync(email) == null)
        {
            var user = new IdentityUser
            {
                Email = email,
                EmailConfirmed = true,
                UserName = email
            };

            var result = await userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, role);
            }
            else
            {
                throw new Exception($"Failed creating user with {user.Email}, Errors:{string.Join(",", result.Errors)}");
            }

        }


    }
}
