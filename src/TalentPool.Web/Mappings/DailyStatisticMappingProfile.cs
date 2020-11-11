using AutoMapper;
using TalentPool.DailyStatistics;
using TalentPool.Web.Models.DailyStatisticViewModels;

namespace TalentPool.Web.Mappings
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
