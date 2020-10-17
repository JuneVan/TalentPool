using System.ComponentModel.DataAnnotations;

namespace Une.TalentPool.Investigations
{
    public enum AcceptTravelStatus : sbyte
    {
        [Display(Name = "愿意")]
        Accept,
        [Display(Name = "不愿意")]
        Refuse,
        [Display(Name = "考虑")]
        Consider
    }
}
