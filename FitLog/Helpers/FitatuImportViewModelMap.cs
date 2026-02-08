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
            Map(m => m.Calories).Name("kalorie (kcal)");
        }
    }
}
