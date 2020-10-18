using AutoMapper;
using System.Linq;
using Une.TalentPool.Application.Resumes;
using Une.TalentPool.Application.Users;
using Une.TalentPool.Resumes;
using Une.TalentPool.Web.Models.ResumeViewModels;

namespace Une.TalentPool.Web.Profiles
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
            CreateMap<ResumeCompare, ResumeCompareDto>();
        }
    }
}
