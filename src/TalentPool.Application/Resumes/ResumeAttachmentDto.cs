using System;

namespace TalentPool.Application.Resumes
{
    public class ResumeAttachmentDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime CreationTime { get; set; } 
    }
}
