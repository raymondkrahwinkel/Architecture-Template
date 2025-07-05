using Domain.Abstractions;
using Domain.Attributes;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Common.Seeders;

[Order(-1)]
internal sealed class InitialSeeder(IServiceProvider serviceProvider) : IDbSeeder 
{
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<UserRoleEntity>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<UserEntity>>();
        
        // define roles
        var roles = new[] { "Admin", "User" };
        foreach (var roleName in roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new UserRoleEntity { Name = roleName });
            }
        }
        
        // define admin user
        const string adminEmail = "administrator@localhost";
        const string adminPassword = "Password123!"; // Ensure this meets your password policy requirements
        
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var adminUser = new UserEntity
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };
            
            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
            else
            {
                throw new Exception("Failed to create the admin user: " + string.Join(", ", result.Errors));
            }
        }
    }
}