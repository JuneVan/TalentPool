using System;
using TalentPool.Entities;

namespace TalentPool.Resumes
{
    public class ResumeAttachment : Entity, ICreationAudited
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public Guid CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public Guid ResumeId { get; set; }
    }
}
