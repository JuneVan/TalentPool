using AutoMapper;
using TalentPool.Application.Resumes;
using TalentPool.Investigations;
using TalentPool.Resumes;
using TalentPool.Web.Models.InvestigationViewModels;

namespace TalentPool.Web.Mappings
{
    public class InvestigationMappingProfile:Profile
    {
        public InvestigationMappingProfile()
        {
            CreateMap<Resume, CreateInvestigationViewModel>()
                .BeforeMap((src, dest) =>
            {
                dest.ResumeId = src.Id;
            });
            CreateMap<CreateInvestigationViewModel, Investigation>();
            CreateMap<Investigation, EditInvestigationViewModel>();
            CreateMap<ResumeDetailDto, EditInvestigationViewModel>()
                .ForMember(m => m.Id, opt => opt.Ignore());
            CreateMap<EditInvestigationViewModel, Investigation>()
                .ForMember(m => m.ResumeId, cfg => cfg.Ignore());
            CreateMap<Investigation, FinshOrRestoreModel>();
            CreateMap<Investigation, AuditInvestigationViewModel>();
            CreateMap<Investigation, DeleteInvestigationViewModel>();
        }
    }
}
