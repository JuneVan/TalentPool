using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Van.TalentPool.Web.Models.AccountViewModels
{
    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }

        public ICollection<SelectListItem> Providers { get; set; }

        public string ReturnUrl { get; set; }

        public bool RememberMe { get; set; }
    }
}
