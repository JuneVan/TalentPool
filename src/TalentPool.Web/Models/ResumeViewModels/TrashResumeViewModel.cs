using System;
using System.ComponentModel.DataAnnotations;

namespace TalentPool.Web.Models.ResumeViewModels
{
    public class TrashResumeViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        [Required(ErrorMessage ="请输入理由")]
        public string EnableReason { get; set; }
    }
}
