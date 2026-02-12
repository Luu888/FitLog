namespace FitLog.Areas.Tracker.ViewModels.Home
{
    public class DaySummaryViewModel
    {
        public int Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset Date { get; set; }
        public decimal CaloriesEaten { get; set; }
        public decimal Steps { get; set; }
        public decimal WorkoutCalories { get; set; }

        public List<MealViewModel> Meals { get; set; }
    }
}
