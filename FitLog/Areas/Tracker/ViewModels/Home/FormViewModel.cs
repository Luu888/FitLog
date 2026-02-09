using FitLog.Models.DatabaseEntities;

namespace FitLog.Areas.Tracker.ViewModels.Home
{
    public class FormViewModel
    {
        public int? Id { get; set; }
        public DateTimeOffset Date { get; set; }
        public decimal Steps { get; set; }
        public decimal WorkoutCalories { get; set; }

        public List<MealViewModel> Meals { get; set; }
    }
}
