using BackendAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace BackendAPI.Auth;

public class AuthDbSeeder
{
    private readonly UserManager<ApartmentUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AuthDbSeeder(UserManager<ApartmentUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }
    
    public async Task SeedAsync()
    {
        await SeedDefaultRoles();
        await SeedAdminUser();
    }

    private async Task SeedDefaultRoles()
    {
        foreach (var role in Roles.All)
        {
            var roleExists = await _roleManager.RoleExistsAsync(role);
            if (!roleExists)
                await _roleManager.CreateAsync(new IdentityRole(role));
        }
    }
    
    private async Task SeedAdminUser()
    {
        var adminUser = new ApartmentUser("admin", "adming@localhost.com");
        var userExists = await _userManager.FindByNameAsync(adminUser.UserName);
        if (userExists != null)
            return;
        
        var createAdminUserResult = await _userManager.CreateAsync(adminUser, "VerySafePassword1!");
        if (createAdminUserResult.Succeeded)
            await _userManager.AddToRolesAsync(adminUser, Roles.All);
        
    }
}