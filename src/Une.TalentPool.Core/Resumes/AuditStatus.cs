using System.ComponentModel.DataAnnotations;

namespace Une.TalentPool.Resumes
{
    public enum AuditStatus
    {
        [Display(Name = "未处理")]
        NoStart = 0,
        [Display(Name = "审核中")]
        Ongoing,
        [Display(Name = "已通过")]
        Complete,
        [Display(Name = "未通过")]
        Unpassed
    }
}
