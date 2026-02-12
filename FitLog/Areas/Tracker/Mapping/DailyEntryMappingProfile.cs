using AutoMapper;
using FitLog.Areas.Tracker.ViewModels.Home;
using FitLog.Models.DatabaseEntities;

namespace FitLog.Areas.Tracker.Mapping
{
    public class DailyEntryMappingProfile : Profile
    {
        public DailyEntryMappingProfile()
        {
            CreateMap<DailyEntry, IndexViewModel>();

            CreateMap<DailyEntry, EditViewModel>();
            CreateMap<EditViewModel, DailyEntry>();

            CreateMap<CreateViewModel, DailyEntry>();

            CreateMap<MealEntry, MealViewModel>();
            CreateMap<MealViewModel, MealEntry>();

            CreateMap<DailyEntry, DaySummaryViewModel>();
            CreateMap<DaySummaryViewModel, DailyEntry>();
        }
    }
}
