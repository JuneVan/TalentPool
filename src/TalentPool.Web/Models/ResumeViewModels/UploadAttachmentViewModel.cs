using System;
using System.Collections.Generic;
using TalentPool.Application.Resumes;

namespace TalentPool.Web.Models.ResumeViewModels
{
    public class UploadAttachmentViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<ResumeAttachmentDto> Attachments { get; set; }
    }
}
