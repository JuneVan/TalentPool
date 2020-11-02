using AutoMapper;
using Une.TalentPool.DailyStatistics;
using Une.TalentPool.Web.Models.DailyStatisticViewModels;

namespace Une.TalentPool.Web.Mappings
{
    public class DailyStatisticMappingProfile: Profile
    {
        public DailyStatisticMappingProfile()
        {
            CreateMap<CreateOrEditDailyStatisticViewModel, DailyStatistic>();
            CreateMap<DailyStatistic, CreateOrEditDailyStatisticViewModel>();
            CreateMap<DailyStatisticItemModel, DailyStatisticItem>();
            CreateMap<DailyStatisticItem, DailyStatisticItemModel>();
            CreateMap<DailyStatistic, DeleteDailyStatisticViewModel>();
        }
    }
}
