using FitLog.Areas.Tracker.ViewModels.Home;
using FitLog.Models.DatabaseEntities;
using FitLog.Services;

namespace FitLog.Areas.Tracker.Services
{
    public interface ITrackerService : IBaseService<DailyEntry>
    {
        Task<DailyEntry> GetSelectedDayAsync(DateTime selectedDate);
        Task<int> ImportAsync(List<ImportViewModel> list);
    }
}
