using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seeder;

public static class Seed
{
    public static async Task SeedAdmin(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
    {
        if (!roleManager.RoleExistsAsync(Roles.Admin.ToString()).Result)
        {
            await roleManager.CreateAsync(new IdentityRole<int>("Admin"));
        }
        var user = userManager.Users.FirstOrDefault(x=>x.UserName == "Admin");
        if (user == null)
        {
            var newUser = new User()
            {
                FirstName = "Admin",
                LastName = "Admin",
                UserName = "Admin",
                PhoneNumber = "123080206",
                Email = "admin@gamil.com",
                Address = "Dushanbe",
                CreatedAt = DateTime.UtcNow,
                UpdateAt = DateTime.UtcNow,
            };
            var result =  userManager.CreateAsync(newUser, "1234Qwerty$");
            if (result.Result.Succeeded)
            {
                await userManager.AddToRoleAsync(newUser, Roles.Admin.ToString());
            }
        }
    }

    public static async Task<bool> SeedRole(RoleManager<IdentityRole<int>> roleManager)
    {
        var newRole = new List<IdentityRole<int>>()
        {
            new(Roles.Admin.ToString()),
            new(Roles.User.ToString()),
        };
        var roles = await roleManager.Roles.ToListAsync();
        foreach (var role in newRole)
        {
            if(roles.Any(x => x.Name == role.Name)) continue;
            await roleManager.CreateAsync(role);
        }
        return true;
    }
}
