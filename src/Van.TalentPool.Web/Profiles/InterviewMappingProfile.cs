using AutoMapper;
using Van.TalentPool.Interviews;
using Van.TalentPool.Resumes;
using Van.TalentPool.Web.Models.InterviewViewModels;

namespace Van.TalentPool.Web.Profiles
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
