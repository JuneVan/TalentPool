using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Une.TalentPool.Application.Investigations;
using Une.TalentPool.Web.Models.CommonModels;

namespace Une.TalentPool.Web.Models.InvestigationViewModels
{
    public class QueryInvestigationViewModel
    {
        public PaginationModel<InvestigationDto> Output { get; set; }
        public List<SelectListItem> Jobs { get; set; }
        public List<SelectListItem> Users { get; set; }
    }
}
