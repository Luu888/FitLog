using FitLog.Models.DatabaseEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FitLog.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<DailyEntry> DailyEntries { get; set; }
        public DbSet<MealEntry> MealEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MealEntry>()
                .HasOne(m => m.DailyEntry)
                .WithMany(d => d.Meals)
                .HasForeignKey(m => m.DailyEntryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DailyEntry>()
                .HasOne(d => d.CreatedByUser)
                .WithMany()
                .HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MealEntry>()
                .HasOne(m => m.CreatedByUser)
                .WithMany()
                .HasForeignKey(m => m.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
