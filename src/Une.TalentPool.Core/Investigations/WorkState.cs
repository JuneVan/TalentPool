using System.ComponentModel.DataAnnotations;

namespace Une.TalentPool.Investigations
{
    public enum WorkState : sbyte
    {
        [Display(Name = "在职")]
        InService,
        [Display(Name = "离职")]
        OutOfService
    }
}
