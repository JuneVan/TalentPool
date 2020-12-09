using AutoMapper;
using TalentPool.Interviews;
using TalentPool.Resumes;
using TalentPool.Web.Models.InterviewViewModels;

namespace TalentPool.Web.Mappings
{
    public class InterviewMappingProfile : Profile
    {
        public InterviewMappingProfile()
        {
            CreateMap<Resume, CreateOrEditInterviewViewModel>()
                 .BeforeMap((src, dest) =>
                 {
                     dest.ResumeId = src.Id;
                 })
                 .ForMember(f => f.Id, opt => opt.Ignore());
            CreateMap<CreateOrEditInterviewViewModel, Interview>();
            CreateMap<Interview, CreateOrEditInterviewViewModel>();
            CreateMap<Interview, ChangeInterviewViewModel>();
            CreateMap<ChangeInterviewViewModel, Interview>();
            CreateMap<Interview, CancelInterviewViewModel>();
        }
    }
}
