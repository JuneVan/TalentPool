using AutoMapper;
using Van.TalentPool.DailyStatistics;
using Van.TalentPool.Web.Models.DailyStatisticViewModels;

namespace Van.TalentPool.Web.Profiles
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
