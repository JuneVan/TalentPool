using System.ComponentModel.DataAnnotations;

namespace TalentPool.Web.Models.AccountViewModels
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required] 
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
