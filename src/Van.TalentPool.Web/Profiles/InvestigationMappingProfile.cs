using AutoMapper;
using Van.TalentPool.Application.Resumes;
using Van.TalentPool.Investigations;
using Van.TalentPool.Resumes;
using Van.TalentPool.Web.Models.InvestigationViewModels;

namespace Van.TalentPool.Web.Profiles
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
            CreateMap<ResumeDetailDto, EditInvestigationViewModel>();
            CreateMap<EditInvestigationViewModel, Investigation>();
            CreateMap<Investigation, FinshOrRestoreModel>();
            CreateMap<Investigation, AuditInvestigationViewModel>();

        }
    }
}
