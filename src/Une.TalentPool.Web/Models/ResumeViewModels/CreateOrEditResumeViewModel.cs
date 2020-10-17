using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Une.TalentPool.Web.Models.ResumeViewModels
{
    public class CreateOrEditResumeViewModel
    {
        public Guid? Id { get; set; }

   
        public List<SelectListItem> Jobs { get; set; }
        [Required(ErrorMessage = "请选择职位")]
        public Guid JobId { get; set; }

        public List<SelectListItem> Platforms { get; set; }
        [Required(ErrorMessage = "请选择招聘平台")]
        public string PlatformName { get; set; }
        [Required(ErrorMessage = "请输入招聘平台ID")]
        public string PlatformId { get; set; }

        public bool ActiveDelivery { get; set; }

        /* Edit ↓ */
        public string Name { get; set; }

        public string City { get; set; }
        public string PhoneNumber { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Description { get; set; }
          
        public string Keywords { get; set; }
    }
}
