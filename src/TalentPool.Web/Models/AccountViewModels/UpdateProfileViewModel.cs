using System.ComponentModel.DataAnnotations;

namespace TalentPool.Web.Models.AccountViewModels
{
    public class UpdateProfileViewModel
    { 
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } 
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Photo { get; set; }
    }
}
