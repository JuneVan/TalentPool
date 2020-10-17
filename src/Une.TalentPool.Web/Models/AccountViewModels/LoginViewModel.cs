using System.ComponentModel.DataAnnotations;

namespace Une.TalentPool.Web.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required] 
        public string UserNameOrEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
