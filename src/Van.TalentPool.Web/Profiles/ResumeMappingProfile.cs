using AutoMapper;
using System.Linq;
using Van.TalentPool.Application.Resumes;
using Van.TalentPool.Application.Users;
using Van.TalentPool.Resumes;
using Van.TalentPool.Web.Models.ResumeViewModels;

namespace Van.TalentPool.Web.Profiles
{
    public class ResumeMappingProfile : Profile
    {
        public ResumeMappingProfile()
        {
            CreateMap<Resume, CreateOrEditResumeViewModel>()
                .AfterMap((src, dest) =>
                {
                    if (src.KeyMaps != null)
                    {
                        dest.Keywords = string.Join(" ", src.KeyMaps.Select(s => s.Keyword));
                    }

                });
            CreateMap<CreateOrEditResumeViewModel, Resume>();
            CreateMap<Resume, DeleteResumeModel>();
            CreateMap<Resume, AssignUserViewModel>();
            CreateMap<UserSelectItemDto, AudtiSettingUserModel>();
            CreateMap<AuditSettingModel, ResumeAuditSetting>();
            CreateMap<ResumeAuditSetting, AuditSettingModel>();
            CreateMap<CreateAuditViewModel, ResumeAuditRecord>();
            CreateMap<ResumeAuditRecordDto, AuditRecordModel>(); 
            CreateMap<Resume, TrashResumeViewModel>();
            CreateMap<Resume, SendEmailViewModel>();
        }
    }
}
