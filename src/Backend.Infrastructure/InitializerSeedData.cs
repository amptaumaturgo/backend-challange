using Backend.Domain.Entities;
using Backend.Shared.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure;

public class InitializerSeedData(ApplicationDbContext applicationDbContext, IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
{
    public async Task ApplyMigrationsAndSeedData()
    {
        await applicationDbContext.Database.MigrateAsync();
        await SeedData();
    }

    private async Task SeedData()
    {
        if (!applicationDbContext.Plans.Any())
        {
            applicationDbContext.Plans.AddRange(new List<Plan>
            {
                new (7, 30.0M),
                new (15, 28.0M),
                new (30, 22.0M),
                new (45, 20.0M),
                new (50, 18.0M),
            });
        }

        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        if (!await roleManager.RoleExistsAsync("Driver"))
        {
            await roleManager.CreateAsync(new IdentityRole("Driver"));
        }

        var adminEmail = "admin@example.com";
        var adminPassword = "Admin@123";

        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var adminUser = new IdentityUser
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
        }

        await unitOfWork.CommitAsync();
    }
}