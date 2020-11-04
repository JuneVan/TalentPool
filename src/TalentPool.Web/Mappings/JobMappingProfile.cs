using AutoMapper;
using TalentPool.Jobs;
using TalentPool.Web.Models.JobViewModels;

namespace TalentPool.Web.Mappings
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
