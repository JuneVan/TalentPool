using System.ComponentModel.DataAnnotations;

namespace Van.TalentPool.Interviews
{
    public enum InterviewStatus : sbyte
    {
        [Display(Name = "预约中")]
        None = 0,
        [Display(Name = "履约")]
        Arrived,
        [Display(Name = "爽约")]
        NotArrived,
        [Display(Name = "取消")]
        Cancel
    }
}
