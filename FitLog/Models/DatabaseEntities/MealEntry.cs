namespace FitLog.Models.DatabaseEntities
{
    public class MealEntry : BaseTable
    {
        public int DailyEntryId { get; set; }
        public DailyEntry DailyEntry { get; set; }

        public string MealName { get; set; }
        public decimal Calories { get; set; }
        public decimal Fat { get; set; }
        public decimal SaturatedFat { get; set; }
        public decimal Carbohydrates { get; set; }
        public decimal Sugars { get; set; }
        public decimal Protein { get; set; }
        public decimal Fiber { get; set; }
        public decimal Salt { get; set; }
    }
}
