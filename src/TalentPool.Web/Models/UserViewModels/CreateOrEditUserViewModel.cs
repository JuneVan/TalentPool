using System;
using System.ComponentModel.DataAnnotations;

namespace TalentPool.Web.Models.UserViewModels
{
    public class CreateOrEditUserViewModel
    {
        public Guid? Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string PhoneNumber { get; set; } 
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Photo { get; set; }
        public bool Confirmed { get; set; }

    }
}
