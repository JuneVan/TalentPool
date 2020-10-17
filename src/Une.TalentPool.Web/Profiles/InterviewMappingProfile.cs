using AutoMapper;
using Une.TalentPool.Interviews;
using Une.TalentPool.Resumes;
using Une.TalentPool.Web.Models.InterviewViewModels;

namespace Une.TalentPool.Web.Profiles
{
    public class InterviewMappingProfile:Profile
    {
        public InterviewMappingProfile()
        {
            CreateMap<Resume, CreateOrEditInterviewViewModel>()
                 .BeforeMap((src, dest) =>
                 {
                     dest.ResumeId = src.Id;
                 });
            CreateMap<CreateOrEditInterviewViewModel, Interview>();
            CreateMap<Interview, CreateOrEditInterviewViewModel>();
            CreateMap<Interview, ChangeInterviewViewModel>();
            CreateMap<ChangeInterviewViewModel, Interview>();
            CreateMap<Interview, CancelInterviewViewModel>();
        }
    }
}
