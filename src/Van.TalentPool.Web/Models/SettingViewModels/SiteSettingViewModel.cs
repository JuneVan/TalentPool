using System.ComponentModel.DataAnnotations;

namespace Van.TalentPool.Web.Models.SettingViewModels
{
    public class SiteSettingViewModel
    {
        [Required(ErrorMessage ="请填写站点名称。")]
        public string SiteName { get; set; }
        public string Logo { get; set; }
    }
}
