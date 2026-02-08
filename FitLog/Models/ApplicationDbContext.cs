using FitLog.Models.DatabaseEntities;
using Microsoft.EntityFrameworkCore;

namespace FitLog.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<DailyEntry> DailyEntries { get; set; }
    }
}
