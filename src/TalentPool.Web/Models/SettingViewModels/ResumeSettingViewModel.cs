using System.ComponentModel.DataAnnotations;

namespace TalentPool.Web.Models.SettingViewModels
{
    public class ResumeSettingViewModel
    {
        [Required(ErrorMessage ="请输入简历匹配最小相似度。")]
        public decimal MinSimilarityValue { get; set; }
    }
}
