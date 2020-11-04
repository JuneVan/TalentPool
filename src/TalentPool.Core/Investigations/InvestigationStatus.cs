using System.ComponentModel.DataAnnotations;

namespace TalentPool.Investigations
{
    public enum InvestigationStatus : sbyte
    {
        [Display(Name = "未开始")]
        NoStart = 0,
        [Display(Name = "进行中")]
        Ongoing = 1,
        [Display(Name = "已完成")]
        Complete,
    }
}
