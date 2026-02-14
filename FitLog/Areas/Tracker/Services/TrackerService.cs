
using FitLog.Areas.Tracker.ViewModels.Home;
using FitLog.Data;
using FitLog.Models;
using FitLog.Models.DatabaseEntities;
using FitLog.Models.Enums.Errors;
using FitLog.Services;
using Microsoft.EntityFrameworkCore;

namespace FitLog.Areas.Tracker.Services
{
    public class TrackerService : BaseService<DailyEntry>, ITrackerService
    {
        private readonly ICurrentUserService _currentUserService;

        public TrackerService(ApplicationDbContext context, ICurrentUserService currentUserService)
            : base(context)
        {
            _currentUserService = currentUserService;
        }

        public override IQueryable<DailyEntry> GetAsQueryable()
        {
            return _context
                .DailyEntries
                .Include(i => i.Meals)
                .Where(x => x.CreatedByUserId == _currentUserService.UserId && !x.IsDeleted);
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
                    CreatedByUserId = _currentUserService.UserId,
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
                        Salt = m.Salt,
                        CreatedByUserId = _currentUserService.UserId,
                        CreatedAt = now
                    }).ToList()
                })
                .ToList();

            var datesToCheck = entries.Select(e => e.Date).ToList();
            var existingDates = await GetAsQueryable()
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

        public override async Task<int> UpdateAsync(int id, DailyEntry entity)
        {
            if (entity?.Id != id)
                return -2;

            var originalEntity = await GetAsQueryable().FirstOrDefaultAsync(x => x.Id == id);

            originalEntity.SetupUpdatedFields(_currentUserService.UserId);

            _context.TryUpdate(originalEntity,
                entity,
                key => key.Id,
                field => field.WorkoutCalories,
                field => field.Steps,
                field => field.UpdatedAt,
                field => field.UpdatedByUserId
                );

            return await _context.SaveChangesAsync();
        }

        public async Task<DailyEntry> GetSelectedDayAsync(DateTime selectedDate)
        {
            var entity = await GetAsQueryable()
                .SingleOrDefaultAsync(x => x.Date.Date == selectedDate.Date);

            if (entity != null)
                return entity;

            entity = new DailyEntry
            {
                Date = selectedDate.Date,
                CreatedByUserId = _currentUserService.UserId,
                CreatedAt = DateTime.Now
            };

            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<int> AddMealAsync(MealEntry entity)
        {
            if (entity == null)
                return (int)MealError.NotFound;

            if (entity.MealName == null || entity.DailyEntryId == 0)
                return (int)MealError.EmptyValues;

            entity.SetupCreatedFields(_currentUserService.UserId);

            await _context.MealEntries.AddAsync(entity);

            return await _context.SaveChangesAsync();
        }
    }
}
