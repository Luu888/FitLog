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
        }
    }
}
