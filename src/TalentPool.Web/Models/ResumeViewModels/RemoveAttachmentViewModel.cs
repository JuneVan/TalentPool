using System;

namespace TalentPool.Web.Models.ResumeViewModels
{
    public class RemoveAttachmentViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid AttachmentId { get; set; }
        public string FileName { get; set; }
    }
}
