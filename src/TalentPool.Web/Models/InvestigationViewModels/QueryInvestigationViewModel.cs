using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using TalentPool.Application.Investigations;
using TalentPool.Web.Models.CommonModels;

namespace TalentPool.Web.Models.InvestigationViewModels
{
    public class QueryInvestigationViewModel
    {
        public PaginationModel<InvestigationDto> Output { get; set; }
        public List<SelectListItem> Jobs { get; set; }
        public List<SelectListItem> Users { get; set; }
    }
}
