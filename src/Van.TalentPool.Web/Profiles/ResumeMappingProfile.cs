using AutoMapper;
using Van.TalentPool.Application.Users;
using Van.TalentPool.Resumes;
using Van.TalentPool.Web.Models.ResumeViewModels;

namespace Van.TalentPool.Web.Profiles
{
    public class ResumeMappingProfile : Profile
    {
        public ResumeMappingProfile()
        {
            CreateMap<Resume, CreateOrEditResumeViewModel>();
            CreateMap<CreateOrEditResumeViewModel, Resume>();
            CreateMap<Resume, DeleteResumeModel>();
            CreateMap<Resume, AssignUserViewModel>();
            CreateMap<CreateAuditViewModel, ResumeAuditRecord>(); 
            CreateMap<UserSelectItemDto, AudtiSettingUserModel>();
            CreateMap<AuditSettingModel, ResumeAuditSetting>();
            CreateMap<ResumeAuditSetting, AuditSettingModel>();

        }
    }
}
