using FitLog.Models;
using FitLog.Models.DatabaseEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FitLog.Seed
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            await context.Database.MigrateAsync();

            if (!await context.DailyEntries.AnyAsync())
            {
                var now = DateTimeOffset.Now;

                var entries = new List<DailyEntry>
                {
                    new() {
                        Date = DateTime.Today.AddDays(-2),
                        Steps = 8500,
                        WorkoutCalories = 300,
                        CreatedAt = now,
                        UpdatedAt = now,
                        IsDeleted = false
                    },
                    new() {
                        Date = DateTime.Today.AddDays(-1),
                        Steps = 10000,
                        WorkoutCalories = 450,
                        CreatedAt = now,
                        UpdatedAt = now,
                        IsDeleted = false
                    },
                    new() {
                        Date = DateTime.Today,
                        Steps = 7500,
                        WorkoutCalories = 250,
                        CreatedAt = now,
                        UpdatedAt = now,
                        IsDeleted = false
                    }
                };

                await context.DailyEntries.AddRangeAsync(entries);
                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedAdminAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            const string adminEmail = "admin@fitlog";
            const string adminPassword = "Admin123!";

            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
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
        }
    }
}
