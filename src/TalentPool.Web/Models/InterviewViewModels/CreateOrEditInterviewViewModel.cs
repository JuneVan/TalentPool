using System;
using System.ComponentModel.DataAnnotations;

namespace TalentPool.Web.Models.InterviewViewModels
{
    public class CreateOrEditInterviewViewModel
    {
        public Guid? Id { get; set; } 
        public Guid ResumeId { get; set; } 
        public string Name { get; set; }
        public string Remark { get; set; }
        [Required]
        public DateTime AppointmentTime { get; set; }
    }
}
