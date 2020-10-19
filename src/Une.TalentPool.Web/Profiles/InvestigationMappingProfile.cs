using AutoMapper;
using Une.TalentPool.Application.Resumes;
using Une.TalentPool.Investigations;
using Une.TalentPool.Resumes;
using Une.TalentPool.Web.Models.InvestigationViewModels;

namespace Une.TalentPool.Web.Profiles
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

        }
    }
}
