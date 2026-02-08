namespace FitLog.Models.DatabaseEntities
{
    public class DailyEntry : BaseTable
    {
        public DateTimeOffset Date { get; set; }
        public decimal CaloriesEaten { get; set; }
        public decimal Steps { get; set; }
        public decimal WorkoutCalories { get; set; }
    }
}
