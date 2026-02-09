
using FitLog.Areas.Tracker.ViewModels.Home;
using FitLog.Models;
using FitLog.Models.DatabaseEntities;
using FitLog.Models.Enums.Errors;
using FitLog.Services;
using Microsoft.EntityFrameworkCore;

namespace FitLog.Areas.Tracker.Services
{
    public class TrackerService : BaseService<DailyEntry>, ITrackerService
    {
        public TrackerService(ApplicationDbContext context)
            : base(context)
        { 
        }

        public override IQueryable<DailyEntry> GetAsQueryable()
        {
            return _context
                .DailyEntries
                .Include(i => i.Meals)
                .Where(x => !x.IsDeleted);
        }

        public async Task<int> ImportAsync(List<ImportViewModel> list)
        {
            var now = DateTime.Now;
            var entries = list
                .GroupBy(x => x.Date.Date)
                .Select(g => new DailyEntry
                {
                    Date = g.Key,
                    CreatedAt = now,
                    Meals = g.Select(m => new MealEntry
                    {
                        MealName = m.MealName,
                        Calories = m.Calories,
                        Fat = m.Fat,
                        SaturatedFat = m.SaturatedFat,
                        Carbohydrates = m.Carbohydrates,
                        Sugars = m.Sugars,
                        Protein = m.Protein,
                        Fiber = m.Fiber,
                        Salt = m.Salt
                    }).ToList()
                })
                .ToList();

            var datesToCheck = entries.Select(e => e.Date).ToList();
            var existingDates = await _context.DailyEntries
                .Where(d => datesToCheck.Any(dc => dc.Date == d.Date.Date && !d.IsDeleted))
                .Select(d => d.Date.Date)
                .ToListAsync();

            if (existingDates.Count != 0)
            {
                return (int)FitatuImportError.AlreadyExists;
            }

            await _context.DailyEntries.AddRangeAsync(entries);

            return await _context.SaveChangesAsync();
        }

    }
}
