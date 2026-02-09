using System.ComponentModel.DataAnnotations.Schema;

namespace FitLog.Models.DatabaseEntities
{
    public class DailyEntry : BaseTable
    {
        public DateTimeOffset Date { get; set; }
        public decimal Steps { get; set; }
        public decimal WorkoutCalories { get; set; }

        public ICollection<MealEntry> Meals { get; set; }


        [NotMapped]
        public decimal CaloriesEaten
        {
            get
            {
                return Meals?.Where(x => !x.IsDeleted)?.Sum(x => x.Calories) ?? 0m;
            }
        }
    }
}
