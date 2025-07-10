using BMS.DAL.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.DAL.DB;

public class RoleSeeder
{
    public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();


        // check Admin role exist
        if (!await roleManager.RoleExistsAsync(Roles.Admin))
        {

            //if not seed admin role
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
        }


        
    }
}
