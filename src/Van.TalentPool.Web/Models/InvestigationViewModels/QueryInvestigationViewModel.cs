using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Van.TalentPool.Application.Investigations;
using Van.TalentPool.Web.Models.CommonModels;

namespace Van.TalentPool.Web.Models.InvestigationViewModels
{
    public class QueryInvestigationViewModel
    {
        public PaginationModel<InvestigationDto> Output { get; set; }
        public List<SelectListItem> Jobs { get; set; }
        public List<SelectListItem> Users { get; set; }
    }
}
