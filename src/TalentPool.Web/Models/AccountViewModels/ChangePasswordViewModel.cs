using System.ComponentModel.DataAnnotations;

namespace TalentPool.Web.Models.AccountViewModels
{
    public class ChangePasswordViewModel
    { 

        [DataType(DataType.Password)]
        [Required]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "两次输入的密码不一致。")]
        public string ConfirmPassword { get; set; }
    }
}
