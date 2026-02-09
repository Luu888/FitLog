using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using FitLog.Areas.Tracker.ViewModels.Home;

namespace FitLog.Helpers
{
    public class FitatuImportViewModelMap : ClassMap<ImportViewModel>
    {
        public FitatuImportViewModelMap()
        {
            Map(m => m.Date).Name("Data");
            Map(m => m.MealName).Name("Produkty i potrawy");
            Map(m => m.Calories).Name("kalorie (kcal)").Default(0m);
            Map(m => m.Fat).Name("Tłuszcze (g)").Default(0m);
            Map(m => m.SaturatedFat).Name("Nasycone (g)").Default(0m);
            Map(m => m.Carbohydrates).Name("Węglowodany (g)").Default(0m);
            Map(m => m.Sugars).Name("Cukry (g)").Default(0m);
            Map(m => m.Protein).Name("Białka (g)").Default(0m);
            Map(m => m.Fiber).Name("Błonnik (g)").Default(0m);
            Map(m => m.Salt).Name("Sól (g)").Default(0m);
        }
    }
}
