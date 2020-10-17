using AutoMapper;
using Une.TalentPool.Jobs;
using Une.TalentPool.Web.Models.JobViewModels;

namespace Une.TalentPool.Web.Profiles
{
    public class JobMappingProfile:Profile
    {
        public JobMappingProfile()
        {
            CreateMap<Job, CreateOrEditJobViewModel>();
            CreateMap<CreateOrEditJobViewModel, Job>();
            CreateMap<Job, DeleteJobViewModel>();
        }
    }
}
