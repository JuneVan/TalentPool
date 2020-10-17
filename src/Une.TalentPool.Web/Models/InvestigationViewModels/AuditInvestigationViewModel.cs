using System;

namespace Une.TalentPool.Web.Models.InvestigationViewModels
{
    public class AuditInvestigationViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsQualified { get; set; }
        public string QualifiedRemark { get; set; }
    }
}
