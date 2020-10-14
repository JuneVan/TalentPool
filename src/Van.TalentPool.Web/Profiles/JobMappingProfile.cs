using AutoMapper;
using Van.TalentPool.Jobs;
using Van.TalentPool.Web.Models.JobViewModels;

namespace Van.TalentPool.Web.Profiles
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
