using System;
using Une.TalentPool.Investigations;

namespace Une.TalentPool.Application.Resumes
{
    public class UncompleteResumeDto
    {
        public Guid Id { get; set; }
        public string PlatformName { get; set; }
        public string PlatformId { get; set; }
        public string Name { get; set; }
        public Guid JobId { get; set; }
        public string JobName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreationTime { get; set; }
        public Guid OwnerUserId { get; set; }
        public string OwnerUserName { get; set; }
        public Guid? InvestigationId { get; set; }
        public InvestigationStatus? Status { get; set; }
        public DateTime? InvestigationDate { get; set; }
    }
}
