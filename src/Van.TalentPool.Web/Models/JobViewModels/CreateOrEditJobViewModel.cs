using System;
using System.ComponentModel.DataAnnotations;

namespace Van.TalentPool.Web.Models.JobViewModels
{
    public class CreateOrEditJobViewModel
    {
        public Guid? Id { get; set; }
        [Required(ErrorMessage = "请输入职位标题")]
        public string Title { get; set; }
        [Required(ErrorMessage = "请输入任职要求")]
        public string Requirements { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string SalaryRange { get; set; }
        public string GenderRange { get; set; } 
        public string AgeRange { get; set; }
        public string Remark { get; set; }
        public bool Enable { get; set; }
    }
}
