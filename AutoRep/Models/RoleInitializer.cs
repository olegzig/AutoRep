using Microsoft.AspNetCore.Identity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRep.Models
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<SUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "admin@gmail.com";
            string password = "adminPassw1";
            if (await roleManager.FindByNameAsync("mananger") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("mananger"));
            }
            if (await roleManager.FindByNameAsync("master") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("master"));
            }
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                SUser admin = new SUser { Email = adminEmail, UserName = adminEmail};
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "mananger");
                }
            }
        }
    }
}
