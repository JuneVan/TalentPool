using AutoMapper;
using System.Linq;
using TalentPool.Application.Resumes;
using TalentPool.Application.Users;
using TalentPool.Resumes;
using TalentPool.Web.Models.ResumeViewModels;

namespace TalentPool.Web.Mappings
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
            CreateMap<CreateOrEditResumeViewModel, Resume>()
                .ForMember(m => m.AuditStatus, cfg => cfg.Ignore())
               .ForMember(m => m.OwnerUserId, cfg => cfg.Ignore());
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
            CreateMap<Resume, UploadAttachmentViewModel>();
            CreateMap<ResumeAttachment, ResumeAttachmentDto>();

            CreateMap<Resume, RemoveAttachmentViewModel>();
        }
    }
}
