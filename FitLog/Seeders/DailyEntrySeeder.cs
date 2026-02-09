using FitLog.Models;
using FitLog.Models.DatabaseEntities;
using Microsoft.EntityFrameworkCore;

namespace FitLog.Seed
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            await context.Database.MigrateAsync();

            if (await context.DailyEntries.AnyAsync())
                return;

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
}
